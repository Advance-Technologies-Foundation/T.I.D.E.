using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common.Logging;
using MrktApolloApp.Dto;
using MrktApolloApp.RestClient;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace MrktApolloApp.VirtualEntityQueryExecutor
{
	[ExcludeFromCodeCoverage] // not part of the solution, just yet
	[DefaultBinding(typeof(IEntityQueryExecutor), Name = "MrktApolloMixedCompanySearchQueryExecutor")]
	public class MrktApolloMixedCompanySearchQueryExecutor : IEntityQueryExecutor
	{

		#region Constants: Private

		private const string SchemaName = "MrktApolloMixedCompanySearch";

		#endregion

		#region Fields: Private

		private static readonly Guid CountUid = Guid.Parse("059063FD-772A-4206-9A2F-FE6387FB42B2");

		/// <summary>
		/// Determines if a given EntitySchemaQueryFilterCollection is a searchBox filter.
		/// It checks for the right side of the filter expression on the collection of filters, and when they are all the same
		/// then the filter is considered a searchBox filter.
		/// </summary>
		private static readonly Func<IEnumerable<EntitySchemaQueryFilter>, bool> IsSearchBoxFilter = filters => {
			string seenParameterValue = string.Empty;
			IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
				= filters as EntitySchemaQueryFilter[] ?? filters.ToArray();
			if (entitySchemaQueryFilters.ToList().Count == 1) {
				return false;
			}
			foreach (EntitySchemaQueryFilter filter in entitySchemaQueryFilters.Where(f => f.RightExpressions.Any())) {
				string pv = filter.RightExpressions[0].ParameterValue.ToString();
				if (seenParameterValue.IsNullOrEmpty()) {
					seenParameterValue = pv;
				} else if (seenParameterValue != pv) {
					return false;
				}
			}
			return true;
		};

		private static readonly Action<UserConnection, Guid, int> SaveCountToRedis
			= (userConnection, accountId, count) => {
				string key = $"ApolloCount.{accountId}";
				userConnection.SessionData[key] = count;
			};

		private static readonly Func<UserConnection, Guid, int> GetCountFromRedis = (userConnection, accountId) => {
			string key = $"ApolloCount.{accountId}";
			object count = userConnection.SessionData[key];
			return int.TryParse(count?.ToString() ?? "", out int countInt) ? countInt : 0;
		};

		/// <summary>
		/// We have no way to tell filters apart. SearchBox and Folders will send the same thing List of filters,
		/// the only way to tell them apart I found is to check if all filters have the same parameter value, then its
		/// searchBox otherwise assume folder. I need to distinguish them because I need to know which filter for REST request
		/// When UI send All, I assume its FullName, otherwise I will take whatever folder sends.
		/// </summary>
		private static readonly Func<EntitySchemaQueryFilterCollection,
			Dictionary<string, IEnumerable<EntitySchemaQueryFilter>>> SeparateFiltersIntoGroups
			= filters => {
				IEnumerable<IEnumerable<EntitySchemaQueryFilter>> filterInstances = filters
					.Where(f => f is EntitySchemaQueryFilterCollection && f.IsEnabled)
					.Select(i => i.GetFilterInstances());

				Dictionary<string, IEnumerable<EntitySchemaQueryFilter>> dict
					= new Dictionary<string, IEnumerable<EntitySchemaQueryFilter>>();
				int ii = 0;

				filterInstances.ForEach(f => {
					IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
						= f as EntitySchemaQueryFilter[] ?? f.ToArray();
					dict.Add(
						IsSearchBoxFilter(entitySchemaQueryFilters) ? $"searchBoxFilter:{ii}" : $"folderFilter:{ii}",
						entitySchemaQueryFilters);
					ii++;
				});
				return dict;
			};

		/// <summary>
		/// Gets filter value from EntitySchemaQueryFilters by key
		/// </summary>
		private static readonly Func<IEnumerable<EntitySchemaQueryFilter>, string, string>
			GetFolderFilterStringValueByKey = (entitySchemaQueryFilters, key) => entitySchemaQueryFilters
					.Where(f => f.IsEnabled && f.LeftExpression.Path == key)
					.Select(f => f.RightExpressions[0].ParameterValue.ToString())
					.FirstOrDefault() ?? string.Empty;

		private static readonly Func<EntitySchemaQueryFilterCollection, Guid>
			GetAccountIdFromFilters = filters => {
				Guid accountId = Guid.Empty;
				filters
					.Where(f => f is EntitySchemaQueryFilter && f.IsEnabled)
					.ForEach(f => {
						_ = f.GetFilterInstances()
							.Where(i => i != null && i.IsEnabled &&
								i.LeftExpression.ExpressionType == EntitySchemaQueryExpressionType.SchemaColumn &&
								i.LeftExpression.Path == "[Account:Id:MrktAccount].Id" && i.RightExpressions.Count == 1)
							.Select(b => b.RightExpressions[0].ParameterValue)
							.FirstOrDefault(p => Guid.TryParse(p?.ToString(), out accountId));
					});
				return accountId;
			};

		/// <summary>
		/// Fetches Name and MrktZoomInfoId from Account entity by Id
		/// Returns tuple of Name and MrktZoomInfoId
		/// </summary>
		private static readonly Func<UserConnection, Guid, (string Name, string Web)> 
			FetchAccountById = (userConnection, accountId) => {
				Entity accountEntity = userConnection.EntitySchemaManager
					.GetInstanceByName("Account").CreateEntity(userConnection);
				bool fetched = accountEntity.FetchFromDB("Id", accountId, new[] {"Name", "Web"});
				return fetched
					? (accountEntity.GetTypedColumnValue<string>("Name"),
						accountEntity.GetTypedColumnValue<string>("Web"))
					: (string.Empty, string.Empty);
			};

		private static readonly
			Func<EntitySchemaQuery, IEnumerable<(string Name, AggregationTypeStrict AggregationType)>>
			GetAggregateColumns = esq => {
				EntitySchemaQueryColumnCollection columns = esq.Columns;
				if (columns.Count == 0) {
					return Array.Empty<(string Path, AggregationTypeStrict AggregationType)>();
				}

				List<(string Path, AggregationTypeStrict AggregationType)> returnCollection =
					new List<(string Path, AggregationTypeStrict AggregationType)>();

				esq.Columns
					.Where(c => c.ValueExpression.Function is EntitySchemaAggregationQueryFunction)
					.ForEach(f => {
						returnCollection.Add((f.Name,
							((EntitySchemaAggregationQueryFunction)f.ValueExpression.Function).AggregationType));
					});
				return returnCollection;
			};

		private static readonly Func<EntitySchemaQuery, IEnumerable<string>> 
			GetColumns = esq => {
			EntitySchemaQueryColumnCollection columns = esq.Columns;
			if (columns.Count == 0) {
				return Array.Empty<string>();
			}
			string[] columnPaths = new string[columns.Count];
			for (int i = 0; i < columns.Count; i++) {
				columnPaths[i] = columns[i].Path;
			}
			return columnPaths;
		};

		private static readonly Dictionary<string, Func<Company, object>> 
			ColumnMapping = new Dictionary<string, Func<Company, object>> {
				{"Name", c => c.Name},
				{"MrktName", c => c.Name},
				{"MrktApolloId", c => c.Id},
				{"MrktWebSite", c => c.WebsiteUrl},
				{"MrktTwitter", c => c.TwitterUrl},
				{"MrktFacebook", c => c.FacebookUrl},
				{"MrktLinkedIn", c => c.LinkedinUrl},
				{"MrktPhone", c => c.Phone},
				{"MrktPrimaryDomain", c => c.PrimaryDomain},
				{"MrktYearEstablished", c => c.FoundedYear}
			};

		private readonly UserConnection _userConnection;
		private readonly ILog _logger;

		#endregion

		#region Constructors: Public

		public MrktApolloMixedCompanySearchQueryExecutor(){
			_userConnection = ClassFactory.Get<UserConnection>();
			_logger = LogManager.GetLogger("MrktApolloMixedCompanySearchQueryExecutor");
		}

		#endregion

		#region Methods: Public

		public EntityCollection GetEntityCollection(EntitySchemaQuery esq){
			Dictionary<string, IEnumerable<EntitySchemaQueryFilter>> filterGroups
				= SeparateFiltersIntoGroups(esq.Filters);

			string nameFilterValue = string.Empty;
			string sbKey = filterGroups.Keys.FirstOrDefault(k => k.StartsWith("searchBoxFilter"));
			if (!sbKey.IsNullOrWhiteSpace() &&
				filterGroups.TryGetValue(sbKey ?? "", out IEnumerable<EntitySchemaQueryFilter> searchBoxFilter)) {
				nameFilterValue
					= searchBoxFilter.FirstOrDefault()?.RightExpressions?.FirstOrDefault()?.ParameterValue.ToString() ??
					string.Empty;
			}

			string jobTitleFilterValue = string.Empty;
			string managementLevel = string.Empty;
			string accountFilterValue = string.Empty;
			string ffKey = filterGroups.Keys.FirstOrDefault(k => k.StartsWith("folderFilter"));
			if (!ffKey.IsNullOrWhiteSpace() &&
				filterGroups.TryGetValue(ffKey ?? "", out IEnumerable<EntitySchemaQueryFilter> folderFilter)) {
				IEnumerable<EntitySchemaQueryFilter> entitySchemaQueryFilters
					= folderFilter as EntitySchemaQueryFilter[] ?? folderFilter.ToArray();
				accountFilterValue = GetFolderFilterStringValueByKey(entitySchemaQueryFilters, "MrktAccountId");
			}

			if (!Guid.TryParse(accountFilterValue, out Guid accountId)) {
				return new EntityCollection(_userConnection, SchemaName); //cannot throw because UI breaks
			}
			//Guid accountId = Guid.Parse(accountFilterValue);
			//Guid accountId = GetAccountIdFromFilters(esq.Filters);
			// if (accountId == Guid.Empty) {
			// 	return new EntityCollection(_userConnection, SchemaName); //cannot throw because UI breaks
			// }

			EntitySchema schema = _userConnection.EntitySchemaManager.GetInstanceByName(SchemaName);
			EntityCollection collection = new EntityCollection(_userConnection, schema);
			IEnumerable<(string Name, AggregationTypeStrict AggregationType)> aggregateColumns
				= GetAggregateColumns(esq);
			if (aggregateColumns.Any()) {
				EntitySchemaColumn countColumn = schema.Columns.FindByCaption("countId");
				EntitySchemaColumn countByUId = schema.Columns.FindByUId(CountUid);
				if (countColumn == null && countByUId == null) {
					DataValueType valueType = new DataValueTypeManager().GetInstanceByName("Integer");
					EntitySchemaColumn col = new EntitySchemaColumn(schema, valueType) {
						Name = "countId",
						UId = CountUid
					};

					try {
						schema.Columns.Add(col);
					} catch (Exception e) {
						_logger.ErrorFormat("Could not add column {0} to schema {1}", col.Name, schema.Name);
					}
				}
					
				Entity entity = schema.CreateEntity(_userConnection);
				entity.SetColumnValue("Id", Guid.NewGuid().ToString());

				try {
					int countValue = GetCountFromRedis(_userConnection, accountId);
					entity.SetColumnValue("countId", countValue);
				} catch (Exception e) {
					_logger.ErrorFormat("Could not SetColumnValue on column countId {0}", e.Message);
				}

				collection.Add(entity);
				return collection;
			}

			(string Name, string Web) account = FetchAccountById(_userConnection, accountId);
			int page = esq.SkipRowCount / esq.RowCount + 1;
			IApolloRestClient client = ApolloApplication.Instance.GetService<IApolloRestClient>();
			MixedCompanySearchResultDto response = client
				.SearchCompanyByTerm(string.IsNullOrWhiteSpace(account.Web) ? account.Name : account.Web,
					false, page, esq.RowCount);
			SaveCountToRedis(_userConnection, accountId, response.Pagination.TotalEntries);
			List<Company> orgs = response.Organizations?.ToList();
			IEnumerable<string> requestedColumns = GetColumns(esq);

			orgs.ForEach(org => {
				Entity entity = schema.CreateEntity(_userConnection);
				entity.SetColumnValue("Id", Guid.NewGuid().ToString());
				entity.SetColumnBothValues(schema.Columns.FindByName("CreatedBy"),
					"410006e1-ca4e-4502-a9ec-e54d922d2c00", "Supervisor");
				entity.SetColumnBothValues(schema.Columns.FindByName("ModifiedBy"),
					"410006e1-ca4e-4502-a9ec-e54d922d2c00", "Supervisor");
				if (requestedColumns != null) {
					foreach (string column in requestedColumns) {
						if (ColumnMapping.TryGetValue(column, out Func<Company, object> value)) {
							entity.SetColumnValue(column, value(org));
						}
					}
				}
				collection.Add(entity);
			});
			return collection;
		}

		#endregion

	}
}