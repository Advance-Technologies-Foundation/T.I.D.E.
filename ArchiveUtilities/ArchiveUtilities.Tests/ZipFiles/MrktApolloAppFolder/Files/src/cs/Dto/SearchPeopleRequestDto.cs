using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	public class SearchPeopleRequestDto
	{

		[JsonProperty("per_page")]
		public int PageLimit { get; set; }
		
		[JsonProperty("page")]
		public int PageNumber { get; set; }
		
		[JsonProperty("organization_ids")]
		public IEnumerable<string> OrganizationIds { get; set; }
		
		public bool ShouldSerializeKeywords() => !string.IsNullOrEmpty(Keywords);

		[JsonProperty("q_keywords")]
		public string Keywords { get; set; }

		public bool ShouldSerializeTitles() => Titles.Any();
		[JsonProperty("person_titles")]
		public IEnumerable<string> Titles { get; set; }
		
		public bool ShouldSerializeDepartments() => Departments.Any();
		[JsonProperty("person_department_or_subdepartments")]
		public IEnumerable<string> Departments { get; set; }
		
		
		public bool ShouldSerializeManagementLevels() => ManagementLevels.Any();
		[JsonProperty("person_seniorities")]
		public IEnumerable<string> ManagementLevels { get; set; }
		
		
		public bool ShouldSerializeEmailStatuses() => EmailStatuses.Any();
		[JsonProperty("contact_email_status")]
		public IEnumerable<string> EmailStatuses { get; set; }
		
		public bool ShouldSerializeSortByColumn() => !string.IsNullOrWhiteSpace(SortByColumn);
		[JsonProperty("sort_by_field")]
		public string SortByColumn { get; set; }
		
		
		public bool ShouldSerializeSortAscending() => !string.IsNullOrWhiteSpace(SortByColumn);
		[JsonProperty("sort_ascending")]
		public bool SortAscending { get; set; }
	}
}