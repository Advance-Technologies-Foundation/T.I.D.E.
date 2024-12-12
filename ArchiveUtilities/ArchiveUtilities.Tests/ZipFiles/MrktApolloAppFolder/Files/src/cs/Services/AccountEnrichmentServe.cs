using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ATF.Repository;
using ATF.Repository.Providers;
using Common.Logging;
using MrktApolloApp.CustomException;
using MrktApolloApp.DomainModels;
using MrktApolloApp.Dto;
using MrktApolloApp.Repository.Models;
using Newtonsoft.Json;
using Terrasoft.Common;
using Terrasoft.Common.Json;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Factories;

namespace MrktApolloApp.Services
{
	internal interface IAccountEnrichmentServe
	{

		#region Methods: Public

		EnrichedAccount GetEnrichedData(string domain);

		#endregion

	}

	internal sealed class AccountEnrichmentServe : IAccountEnrichmentServe
	{

		#region Fields: Private

		private static readonly Func<OrganizationEnrichmentDto, IDataProvider, EnrichedAccount>
			MapToEnrichedAccount = (dto, dataProvider) => new EnrichedAccount {
				ApolloId = dto.Organization.Id,
				Name = dto.Organization.Name,
				WebSiteUrl = dto.Organization.WebsiteUrl ?? string.Empty,
				BlogUrl = dto.Organization.BlogUrl ?? string.Empty,
				AngellistUrl = dto.Organization.AngelListUrl ?? string.Empty,
				LinkedinUrl = dto.Organization.LinkedinUrl ?? string.Empty,
				TwitterUrl = dto.Organization.TwitterUrl ?? string.Empty,
				FacebookUrl = dto.Organization.FacebookUrl ?? string.Empty,
				Phone = dto.Organization.SanitizedPhone ?? string.Empty,
				LogoUrl = dto.Organization.LogoUrl ?? string.Empty,
				Description = dto.Organization.ShortDescription ?? string.Empty,
				AccountAnnualRevenue = FindRevenueByNumber(dto.Organization.AnnualRevenue),
				AccountEmployeeNumber = FindEmployeeCountByNumber(dto.Organization.EstimatedNumEmployees),
				AccountIndustry = FindFirstMatchingIndustryByName(dto.Organization.Industries),
				StreetAddress = dto.Organization.StreetAddress ?? string.Empty,
				City = GetCityByName(dataProvider, dto.Organization.City)?.Id ?? Guid.Empty,
				Region = GetRegionByName(dataProvider, dto.Organization.State)?.Id ?? Guid.Empty,
				Country = GetCountryByName(dataProvider, dto.Organization.Country)?.Id ?? Guid.Empty,
				PostalCode = dto.Organization.PostalCode ?? string.Empty,
				Domain = dto.Organization.PrimaryDomain ?? string.Empty
			};

		private static readonly Func<IDataProvider, string, City> GetCityByName
			= (dataProvider, name) => AppDataContextFactory.GetAppDataContext(dataProvider)
				.Models<City>()
				.FirstOrDefault(c => c.Name == name);

		private static readonly Func<IDataProvider, string, Country> GetCountryByName
			= (dataProvider, name) => AppDataContextFactory.GetAppDataContext(dataProvider)
				.Models<Country>()
				.FirstOrDefault(c => c.Name == name);

		private static readonly Func<IDataProvider, string, Region> GetRegionByName
			= (dataProvider, name) => AppDataContextFactory.GetAppDataContext(dataProvider)
				.Models<Region>()
				.FirstOrDefault(c => c.Name == name);

		private static readonly Func<IEnumerable<string>, Guid> FindFirstMatchingIndustryByName = industries => {
			IEnumerable<string> enumerable = industries as string[] ?? industries.ToArray();
			if (!enumerable.Any()) {
				return Guid.Empty;
			}
			foreach (string industry in enumerable) {
				Select select = new Select(ClassFactory.Get<UserConnection>())
					.From("MrktApolloIndustry").WithHints(new NoLockHint())
					.Column("MrktAccountIndustryId")
					.Where(Func.Upper("Name")).IsEqual(Column.Parameter(industry.ToUpper())) as Select;
				Guid mayBeId = select?.ExecuteScalar<Guid>() ?? Guid.Empty;
				if (mayBeId != Guid.Empty) {
					return mayBeId;
				}
			}
			return Guid.Empty;
		};

		private static readonly Func<decimal, Guid> FindRevenueByNumber = revenue => {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Select max = new Select(userConnection)
				.From("AccountAnnualRevenue").WithHints(new NoLockHint())
				.Top(1).Column("Id").Column("FromBaseCurrency")
				.OrderBy(OrderDirectionStrict.Descending, "FromBaseCurrency") as Select;
			
			Guid id = Guid.Empty;
			int maxValue = 0;
			using (DBExecutor dbExecutor = userConnection.EnsureDBConnection()) {
				using (IDataReader reader = max.ExecuteReader(dbExecutor)) {
					while (reader.Read()) {
						maxValue = reader.GetColumnValue<int>("FromBaseCurrency");
						id = reader.GetColumnValue<Guid>("Id");
					}
				}
			}
			if (revenue >= maxValue) {
				return id;
			}

			Select select = new Select(userConnection)
				.From("AccountAnnualRevenue").WithHints(new NoLockHint())
				.Column("Id")
				.Where("FromBaseCurrency").IsLessOrEqual(Column.Parameter(revenue))
				.And("ToBaseCurrency").IsGreaterOrEqual(Column.Parameter(revenue)) as Select;
			return select?.ExecuteScalar<Guid>() ?? Guid.Empty;
		};

		private static readonly Func<int, Guid> FindEmployeeCountByNumber = count => {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Select max = new Select(userConnection)
				.From("MrktApolloAccountEmployeesNumber").WithHints(new NoLockHint())
				.Top(1)
				.Column("MrktCreatioAccountEmployeesNumberId")
				.Column("MrktFrom")
				.OrderBy(OrderDirectionStrict.Descending, "MrktFrom") as Select;
			
			Guid id = Guid.Empty;
			int maxValue = 0;
			using (DBExecutor dbExecutor = userConnection.EnsureDBConnection()) {
				using (IDataReader reader = max.ExecuteReader(dbExecutor)) {
					while (reader.Read()) {
						maxValue = reader.GetColumnValue<int>("MrktFrom");
						id = reader.GetColumnValue<Guid>("MrktCreatioAccountEmployeesNumberId");
					}
				}
			}
			if (count >= (decimal)maxValue) {
				return id;
			}

			Select select = new Select(userConnection)
				.From("MrktApolloAccountEmployeesNumber").WithHints(new NoLockHint())
				.Column("MrktCreatioAccountEmployeesNumberId")
				.Where("MrktFrom").IsLessOrEqual(Column.Parameter(count))
				.And("MrktTo").IsGreaterOrEqual(Column.Parameter(count)) as Select;
			return select?.ExecuteScalar<Guid>() ?? Guid.Empty;
		};

		private readonly ILog _logger;
		private readonly IDataProvider _dataProvider;
		private readonly HttpClient _httpClient;

		#endregion

		#region Constructors: Public

		public AccountEnrichmentServe(IHttpClientFactory factory, ILog logger, IDataProvider dataProvider){
			_logger = logger;
			_dataProvider = dataProvider;
			_httpClient = factory.CreateClient("ApolloApi");
		}

		#endregion

		#region Methods: Private

		private EnrichedAccount Deserialize(string json){
			JsonSerializerSettings settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			};
			try {
				OrganizationEnrichmentDto enrichedOrg = Json.Deserialize<OrganizationEnrichmentDto>(json, settings);
				if (enrichedOrg.Organization != null) {
					return MapToEnrichedAccount(enrichedOrg, _dataProvider);
				}
				return new EnrichedAccount();
			} catch (Exception e) {
				_logger.ErrorFormat("[ERROR] - {0}\r\nAccountEnrichmentServe could not deserialize json: {1}",
					e.Message, json);
				return new EnrichedAccount();
			}
		}

		[Pure]
		private async Task<EnrichedAccount> GetEnrichedDataInternal(string domain){
			HttpResponseMessage response = await _httpClient.GetAsync($"v1/organizations/enrich?domain={domain}");
			
			
			if(response.StatusCode == (HttpStatusCode)699) {
				return new EnrichedAccount() {
					ErrorMessage = Messages.ApiKeyMissing
				};
			}
			
			if(response.StatusCode == HttpStatusCode.Unauthorized) {
				return new EnrichedAccount() {
					ErrorMessage = Messages.Unauthorized
				};
			}
			
			if((int)response.StatusCode == 422) {
				return new EnrichedAccount() {
					ErrorMessage = Messages.InsufficientCredits
				};
			}
			
			if(response.StatusCode == HttpStatusCode.Forbidden) {
				return new EnrichedAccount() {
					ErrorMessage = Messages.Forbidden 
				};
			}
			if (response.StatusCode != HttpStatusCode.OK) {
				return new EnrichedAccount() {
					ErrorMessage = Messages.Unknown
				};
			}
			
			
			if (response.StatusCode == HttpStatusCode.OK) {
				string jsonContent = await response.Content.ReadAsStringAsync()
					.ConfigureAwait(false);
				return Deserialize(jsonContent);
			}
			return new EnrichedAccount();
		}

		#endregion

		#region Methods: Public

		[Pure]
		public EnrichedAccount GetEnrichedData(string domain) =>
			GetEnrichedDataInternal(domain).GetAwaiter().GetResult();

		#endregion

	}
}