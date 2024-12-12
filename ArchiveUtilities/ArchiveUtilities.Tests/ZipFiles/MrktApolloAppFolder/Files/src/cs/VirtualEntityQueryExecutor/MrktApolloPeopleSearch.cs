using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MrktApolloApp.Dto;
using MrktApolloApp.Services;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Factories;
using Terrasoft.Core.Factories;

namespace MrktApolloApp.VirtualEntityQueryExecutor
{
	[DefaultBinding(typeof(IEntityQueryExecutor), Name = "MrktApolloPeopleSearchQueryExecutor")]
	public class MrktApolloPeopleSearchQueryExecutor : IEntityQueryExecutor
	{

		#region Constants: Private

		private const string SchemaName = "MrktApolloPeopleSearch";

		#endregion

		#region Fields: Private

		private readonly UserConnection _userConnection;
		private readonly EntityCollection _collection;

		#endregion

		#region Constructors: Public

		public MrktApolloPeopleSearchQueryExecutor(){
			_userConnection = ClassFactory.Get<UserConnection>();
			_collection = new EntityCollection(_userConnection, SchemaName);
		}

		#endregion

		#region Methods: Private

		private EntityCollection MapPersonEnrichmentServiceResponseToEntityCollection(
			(IEnumerable<Person> people, IEnumerable<Contact> contacts) response, EntitySchema schema){
			EntityCollection collection = new EntityCollection(_userConnection, schema.Name);
			IEntityMapper mapper = ApolloApplication.Instance.GetService<IEntityMapper>();
			foreach (Person person in response.people) {
				Entity entity = mapper.MapDtoToEntity(person, schema);
				if (entity != null) {
					collection.Add(entity);
				}
			}
			foreach (Contact contact in response.contacts) {
				Entity entity = mapper.MapDtoToEntity(contact, schema);
				if (entity != null) {
					collection.Add(entity);
				}
			}
			return collection;
		}

		#endregion

		#region Methods: Public

		public EntityCollection GetEntityCollection(EntitySchemaQuery esq){
			if (esq.SkipRowCount % 100 != 0) {
				return _collection;
			}
			IEsqFilterParser esqFilterParser = ApolloApplication.Instance.GetService<IEsqFilterParser>();
			IEnumerable<string> organizationId = GetOrganizationIdFromEsq(esqFilterParser, esq);
			if(organizationId == null || organizationId.Count()!=1 ) {
				return _collection;
			}
			IEntityMapper mapper = ApolloApplication.Instance.GetService<IEntityMapper>();
			IEnumerable<string> emailStatuses = GetEmailStatusFromEsq(mapper, esqFilterParser, esq);
			IEnumerable<string> managementLevels = GetManagementLevelFromEsq(mapper, esqFilterParser, esq);
			IEnumerable<string> departments = GetDepartmentFromEsq(mapper, esqFilterParser, esq);
			IEnumerable<string> titleFilter = GetTitlesFromEsq(esqFilterParser, esq);
			string keywordsFilterValue = esqFilterParser.GetSearchBoxFilterValue(esq);
			
			//Get Sorting column and direction
			IEsqColumnParser columnParser = ApolloApplication.Instance.GetService<IEsqColumnParser>();
			SortColumn sortColumn = columnParser.GetSorting(esq);
			string sortField = mapper.MapSortColumnToSortField(sortColumn);
			bool sortAscending = sortColumn?.OrderDirection == OrderDirection.Ascending;
			
			//Make REST Call
			var dto = new SearchPeopleRequestDto() {
				PageLimit = 100,
				PageNumber = 1 + esq.SkipRowCount / 100,
				OrganizationIds = organizationId,
				Keywords = keywordsFilterValue,
				Titles = titleFilter,
				Departments = departments,
				ManagementLevels = managementLevels,
				EmailStatuses = emailStatuses,
				SortAscending = sortAscending,
				SortByColumn = sortField
			};
			IPersonEnrichmentService personEnrichmentService = ApolloApplication.Instance.GetService<IPersonEnrichmentService>();
			(IEnumerable<Person> people, IEnumerable<Contact> contacts) = personEnrichmentService
				.SearchedPeopleAndContactsInOrganization(dto);
			
			//Map back to EntityCollection
			EntitySchema schema = _userConnection.EntitySchemaManager.FindInstanceByName(SchemaName);
			EntityCollection collection = MapPersonEnrichmentServiceResponseToEntityCollection((people, contacts), schema);
			if(sortColumn != null && collection!=null && collection.Count > 0) {
				if(esq.Columns.FindByName(sortColumn.Name).IsLookup) {
					collection.Order($"{sortColumn.Name}Id", sortColumn.OrderDirection);
				}else {
					collection.Order(sortColumn.Name, sortColumn.OrderDirection);
				}
			}
			return collection;
		}

		#endregion
		
		private IEnumerable<string> GetEmailStatusFromEsq(IEntityMapper mapper, IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			IList<string> emailStatusIds = esqFilterParser
				.GetEsqFilterValueByKey2("MrktEmailStatusId", esq).ToList();
			
			IList<string> emailStatusName = esqFilterParser
				.GetEsqFilterValueByKey2("MrktEmailStatus.Name", esq).ToList();
			
			if(!emailStatusIds.Any() && !emailStatusName.Any()) {
				return Array.Empty<string>();
			}
			
			bool allAreGuids = emailStatusIds.Any() && emailStatusIds.All(id => Guid.TryParse(id, out Guid _));
			if(allAreGuids) {
				return mapper.MapEmailStatusIdsToNames(emailStatusIds);
			}
			
			if(!emailStatusName.Any()) {
				return Array.Empty<string>();
			}
			return mapper
				.MapEmailStatusDisplayValueToApolloCode(
						emailStatusName
							.FirstOrDefault()
							?.Split(',')
				);
		}
		
		[ExcludeFromCodeCoverage]
		private IEnumerable<string> GetManagementLevelFromEsq(IEntityMapper mapper, IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			IList<string> ids = esqFilterParser
				.GetEsqFilterValueByKey2("MrktApolloManagementLevelId", esq).ToList();
			
			IList<string> name = esqFilterParser
				.GetEsqFilterValueByKey2("MrktApolloManagementLevel.Name", esq).ToList();
			
			if(!ids.Any() && !name.Any()) {
				return Array.Empty<string>();
			}
			
			bool allAreGuids = ids.Any() && ids.All(id => Guid.TryParse(id, out Guid _));
			if(allAreGuids) {
				return mapper.MapManagementLevelIdsToNames(ids);
			}
			
			return mapper
				.MapManagementLevelDisplayValueToApolloCode(
					name
						.FirstOrDefault()
						?.Split(',')
				);
		}
		
		[ExcludeFromCodeCoverage]
		private IEnumerable<string> GetDepartmentFromEsq(IEntityMapper mapper, IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			IList<string> ids = esqFilterParser
				.GetEsqFilterValueByKey2("MrktApolloDepartmentId", esq).ToList();
			
			IList<string> name = esqFilterParser
				.GetEsqFilterValueByKey2("MrktApolloDepartment.Name", esq).ToList();
			
			if(!ids.Any() && !name.Any()) {
				return Array.Empty<string>();
			}
			
			bool allAreGuids = ids.Any() && ids.All(id => Guid.TryParse(id, out Guid _));
			if(allAreGuids) {
				return mapper.MapDepartmentIdsToNames(ids);
			}
			
			if(!name.Any()) {
				return Array.Empty<string>();
			}
			
			return mapper
				.MapMapDepartmentDisplayValueToApolloCode(
					name
						.FirstOrDefault()
						?.Split(',')
				);
		}
		
		[ExcludeFromCodeCoverage]
		private IList<string> GetOrganizationIdFromEsq(IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			return esqFilterParser
				.GetEsqFilterValueByKey2("MrktOrganizationId", esq).ToList();
		}
		
		
		private IEnumerable<string> GetTitlesFromEsq(IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			string titleFilterValue = esqFilterParser.GetEsqFilterValueByKey("MrktTitle", esq);
			return string.IsNullOrWhiteSpace(titleFilterValue)
				? Array.Empty<string>()
				: titleFilterValue.Split(',');
		}
	}
}