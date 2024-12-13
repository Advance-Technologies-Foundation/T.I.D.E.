using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	internal class Person : IMappableDto
	{

		#region Properties: Public

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("contact")]
		public Contact Contact { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("departments")]
		public IEnumerable<string> Departments { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("email_status")]
		public string EmailStatus { get; set; }

		[JsonProperty("employment_history")]
		public IEnumerable<EmploymentHistory> EmploymentHistory { get; set; }

		[JsonProperty("extrapolated_email_confidence")]
		public object ExtrapolatedEmailConfidence { get; set; }

		[JsonProperty("facebook_url")]
		public string FacebookUrl { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("functions")]
		public IEnumerable<string> Functions { get; set; }

		[JsonProperty("github_url")]
		public string GithubUrl { get; set; }

		[JsonProperty("hashed_email")]
		public string HashedEmail { get; set; }

		[JsonProperty("headline")]
		public string Headline { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("intent_strength")]
		public object IntentStrength { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("linkedin_url")]
		public string LinkedinUrl { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("organization")]
		public Organization Organization { get; set; }

		[JsonProperty("organization_id")]
		public string OrganizationId { get; set; }

		[JsonProperty("personal_emails")]
		public IEnumerable<string> PersonalEmails { get; set; }

		[JsonProperty("phone_numbers")]
		public IEnumerable<PhoneNumbers> PhoneNumbers { get; set; }

		[JsonProperty("photo_url")]
		public string PhotoUrl { get; set; }

		[JsonProperty("revealed_for_current_team")]
		public bool RevealedForCurrentTeam { get; set; }

		[JsonProperty("seniority")]
		public string Seniority { get; set; }

		[JsonProperty("show_intent")]
		public bool ShowIntent { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("subdepartments")]
		public IEnumerable<string> Subdepartments { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("twitter_url")]
		public string TwitterUrl { get; set; }

		#endregion

	}
}