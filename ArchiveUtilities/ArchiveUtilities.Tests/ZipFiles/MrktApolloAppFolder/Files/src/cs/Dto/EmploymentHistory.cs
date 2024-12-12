using System;
using System.Diagnostics.CodeAnalysis;
using MrktApolloApp.Dto.Converters;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	internal class EmploymentHistory
	{
		[JsonProperty("_id")]
		public string Id { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("current")]
		public bool Current { get; set; }

		[JsonProperty("degree")]
		public object Degree { get; set; }

		[JsonProperty("description")]
		public object Description { get; set; }

		[JsonProperty("emails")]
		public object Emails { get; set; }

		[JsonProperty("end_date")]
		public DateTime EndDate { get; set; }

		[JsonProperty("grade_level")]
		public object GradeLevel { get; set; }

		[JsonProperty("kind")]
		public object Kind { get; set; }

		[JsonProperty("major")]
		public object Major { get; set; }

		[JsonProperty("organization_id")]
		public string OrganizationId { get; set; }

		[JsonProperty("organization_name")]
		public string OrganizationName { get; set; }

		[JsonProperty("raw_address")]
		public object RawAddress { get; set; }

		[JsonConverter(typeof(ShortDateConverter))]
		[JsonProperty("start_date")]
		public DateTime StartDate { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("id")]
		public string Id2 { get; set; }

		[JsonProperty("key")]
		public string Key { get; set; }
	}
}