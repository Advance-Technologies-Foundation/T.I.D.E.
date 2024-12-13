using System;
using System.Collections.Generic;
using System.Linq;
using ATF.Repository;
using ATF.Repository.Providers;
using MrktApolloApp.Dto;
using MrktApolloApp.Repository.Models;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Contact = MrktApolloApp.Dto.Contact;

namespace MrktApolloApp.VirtualEntityQueryExecutor
{
	/// <summary>
	/// The IEntityMapper interface defines the contract for mapping data transfer objects (DTOs) to entities.
	/// </summary>
	public interface IEntityMapper
	{
		/// <summary>
		/// Maps a given DTO to an entity.
		/// </summary>
		/// <param name="dto">The DTO to be mapped to an entity.</param>
		/// <param name="schema">The schema of the entity to which the DTO will be mapped.</param>
		/// <returns>The mapped entity.</returns>
		Entity MapDtoToEntity(IMappableDto dto, EntitySchema schema);
		IEnumerable<string> MapDepartmentIdsToNames(IEnumerable<string> departmentIds);
		IEnumerable<string> MapManagementLevelIdsToNames(IEnumerable<string> managementLevelIds);
		IEnumerable<string> MapEmailStatusIdsToNames(IEnumerable<string> emailStatusIds);
		IEnumerable<string> MapEmailStatusDisplayValueToApolloCode(IEnumerable<string>  displayValues);
		IEnumerable<string> MapManagementLevelDisplayValueToApolloCode(IEnumerable<string>  displayValues);
		IEnumerable<string> MapMapDepartmentDisplayValueToApolloCode(IEnumerable<string>  displayValues);
		string MapSortColumnToSortField(SortColumn column);

	}

	public class EntityMapper : IEntityMapper
	{
		private readonly UserConnection _userConnection;
		private readonly IList<MrktApolloManagementLevel> _managementLevels;
		private readonly IList<MrktApolloEmailStatus> _emailStatuses;
		private readonly IList<MrktApolloDepartment> _departments;

		public EntityMapper(UserConnection userConnection, IDataProvider dataProvider){
			_userConnection = userConnection;
			IAppDataContext ctx = AppDataContextFactory.GetAppDataContext(dataProvider);
			_managementLevels = ctx.Models<MrktApolloManagementLevel>().ToList();
			_emailStatuses = ctx.Models<MrktApolloEmailStatus>().ToList();
			_departments = ctx.Models<MrktApolloDepartment>().ToList();
		}
		public Entity MapDtoToEntity(IMappableDto dto, EntitySchema schema) {
			switch (dto) {
				case Person person:
					return MapPersonToEntity(person, schema);
				case Contact contact:
					return MapContactToEntity(contact, schema);
				default:
					throw new ArgumentException("Unsupported dto type", nameof(dto));
			}
		}
		
		private Entity MapDtoInternal(EntitySchema schema){
			Entity entity = schema.CreateEntity(_userConnection);
			Guid id = Guid.NewGuid();
			entity.SetColumnValue("Id", id);
			entity.SetColumnBothValues(schema.Columns.FindByName("CreatedBy"),
				"410006e1-ca4e-4502-a9ec-e54d922d2c00", "Supervisor");
			entity.SetColumnBothValues(schema.Columns.FindByName("ModifiedBy"),
				"410006e1-ca4e-4502-a9ec-e54d922d2c00", "Supervisor");
			return entity;
		}
		
		private Entity MapPersonToEntity(Person person, EntitySchema schema){
			Entity entity = MapDtoInternal(schema);
			entity.SetColumnValue("MrktFirstName", person.FirstName);
			entity.SetColumnValue("MrktLastName", person.LastName);
			entity.SetColumnValue("MrktOrganizationId", person.OrganizationId);
			entity.SetColumnValue("MrktPersonId", person.Id);
			entity.SetColumnValue("MrktTitle", person.Title);
			entity.SetColumnValue("MrktLastEmploymentTitle", person.EmploymentHistory?.FirstOrDefault()?.Title ?? string.Empty);
			entity.SetColumnValue("MrktLastEmploymentStartDate", person.EmploymentHistory?.FirstOrDefault()?.StartDate ?? DateTime.Now.Date);
			entity.SetColumnValue("MrktFacebookUrl", person.FacebookUrl ?? string.Empty);
			entity.SetColumnValue("MrktLinkedinUrl", person.LinkedinUrl ?? string.Empty);
			entity.SetColumnValue("MrktTwitterUrl", person.TwitterUrl ?? string.Empty);
			
			MrktApolloManagementLevel seniority = _managementLevels
				.FirstOrDefault(m=> m.ApolloCode == person.Seniority);
			entity.SetColumnBothValues(schema.Columns.FindByName("MrktApolloManagementLevel"),
				seniority?.Id ?? Guid.Empty, seniority?.Description ?? string.Empty);
			
			MrktApolloEmailStatus apolloEmailStatus = _emailStatuses
				.FirstOrDefault(m=> m.ApolloCode == person.EmailStatus);
			entity.SetColumnBothValues(schema.Columns.FindByName("MrktEmailStatus"),
				apolloEmailStatus?.Id ?? Guid.Empty, apolloEmailStatus?.Name ?? string.Empty);
			
			MrktApolloDepartment apolloDepartment = _departments
				.FirstOrDefault(m=> m.ApolloCode == (person.Departments?.FirstOrDefault() ?? string.Empty));
			entity.SetColumnBothValues(schema.Columns.FindByName("MrktApolloDepartment"),
				apolloDepartment?.Id ?? Guid.Empty, apolloDepartment?.Name ?? string.Empty);
			
			return entity;
		}
		private Entity MapContactToEntity(Contact contact, EntitySchema schema){
			Entity entity = MapDtoInternal(schema);
			entity.SetColumnValue("MrktFirstName", contact.FirstName);
			entity.SetColumnValue("MrktLastName", contact.LastName);
			entity.SetColumnValue("MrktOrganizationId", contact.OrganizationId);
			entity.SetColumnValue("MrktPersonId", contact.PersonId);
			entity.SetColumnValue("MrktTitle", contact.Title);
			entity.SetColumnValue("MrktLastEmploymentTitle", contact.EmploymentHistory?.FirstOrDefault()?.Title ?? string.Empty);
			entity.SetColumnValue("MrktLastEmploymentStartDate", contact.EmploymentHistory?.FirstOrDefault()?.StartDate ?? DateTime.Now.Date);
			entity.SetColumnValue("MrktFacebookUrl", contact.FacebookUrl ?? string.Empty);
			entity.SetColumnValue("MrktLinkedinUrl", contact.LinkedinUrl ?? string.Empty);
			entity.SetColumnValue("MrktTwitterUrl", contact.TwitterUrl ?? string.Empty);
			
			MrktApolloEmailStatus apolloEmailStatus = _emailStatuses
				.FirstOrDefault(m=> m.ApolloCode == contact.EmailStatus);
			
			entity.SetColumnBothValues(schema.Columns.FindByName("MrktEmailStatus"),
				apolloEmailStatus?.Id ?? Guid.Empty, apolloEmailStatus?.Name ?? string.Empty);
			
			return entity;
		}
		public IEnumerable<string> MapDepartmentIdsToNames(IEnumerable<string> departmentIds){
			var collection = new List<string>();
			foreach (var departmentId in departmentIds) {
				bool isGuid = Guid.TryParse(departmentId,  out Guid deptId);
				if(isGuid) {
					var items = _departments
						.Where(d=> d.Id == deptId)
						.Select(d=>d.ApolloCode);
					collection.AddRange(items);
				}
			}
			return collection;
		}
		public IEnumerable<string> MapManagementLevelIdsToNames(IEnumerable<string> managementLevelIds){
			var collection = new List<string>();
			foreach (var managementLevelId in managementLevelIds) {
				bool isGuid = Guid.TryParse(managementLevelId,  out Guid id);
				if(isGuid) {
					var items = _managementLevels
						.Where(d=> d.Id == id)
						.Select(d=>d.ApolloCode);
					collection.AddRange(items);
				}
			}
			return collection;
		}
		public IEnumerable<string> MapEmailStatusIdsToNames(IEnumerable<string> emailStatusIds){
			var collection = new List<string>();
			foreach (var emailStatusId in emailStatusIds) {
				bool isGuid = Guid.TryParse(emailStatusId,  out Guid deptId);
				if(isGuid) {
					var items = _emailStatuses
						.Where(d=> d.Id == deptId)
						.Select(d=>d.ApolloCode);
					collection.AddRange(items);
				}
			}
			return collection;
		}
		public string MapSortColumnToSortField(SortColumn column){
			if(column?.Name == null) {
				return string.Empty;
			}
			switch (column.Name) {
				case "MrktFirstName": 
				case "MrktLastName": 
					return "person_name.raw";
				case "MrktTitle":
					return "person_title_normalized";
				case "MrktEmail":
					return "person_email";
				default:
					return string.Empty;
			}
		}
		
		public IEnumerable<string> MapEmailStatusDisplayValueToApolloCode(IEnumerable<string>  displayValues){
			IEnumerable<string> enumerable = displayValues as string[] ?? displayValues.ToArray();
			if(!enumerable.Any()) {
				return Array.Empty<string>();
			}
			List<string> collection = new List<string>();
			foreach (string displayValue in enumerable) {
				var items = _emailStatuses
					.Where(d=> d.Name == displayValue.Trim())
					.Select(d=>d.ApolloCode);
				collection.AddRange(items);
			}
			return collection;
		}
		
		public IEnumerable<string> MapManagementLevelDisplayValueToApolloCode(IEnumerable<string>  displayValues){
			IEnumerable<string> enumerable = displayValues as string[] ?? displayValues.ToArray();
			if(!enumerable.Any()) {
				return Array.Empty<string>();
			}
			List<string> collection = new List<string>();
			foreach (string displayValue in enumerable) {
				var items = _managementLevels
					.Where(d=> d.Description == displayValue.Trim())
					.Select(d=>d.ApolloCode);
				
				collection.AddRange(items);
			}
			return collection;
		}
		
		public IEnumerable<string> MapMapDepartmentDisplayValueToApolloCode(IEnumerable<string>  displayValues){
			IEnumerable<string> enumerable = displayValues as string[] ?? displayValues.ToArray();
			if(!enumerable.Any()) {
				return Array.Empty<string>();
			}
			List<string> collection = new List<string>();
			foreach (string displayValue in enumerable) {
				var items = _departments
					.Where(d=> d.Name == displayValue.Trim())
					.Select(d=>d.ApolloCode);
				collection.AddRange(items);
			}
			return collection;
		}
	}
}