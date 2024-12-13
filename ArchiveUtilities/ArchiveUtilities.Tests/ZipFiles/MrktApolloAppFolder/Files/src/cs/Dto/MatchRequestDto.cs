using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class MatchRequestDto
	{

		[JsonProperty("id")]
		public string Id { get; set; }
		
		[JsonProperty("reveal_personal_emails")]
		public bool RevealPersonalEmails { get; } = true;
		
		[JsonProperty("organization_name")]
		public string OrganizationName { get; set;}
		
		[JsonProperty("website_url")]
		public string OrganizationDomain { get; set;}
		
		[JsonProperty("first_name")]
		public string FirstName { get; set;}
		
		[JsonProperty("last_name")]
		public string LastName { get; set;}
		
		[JsonProperty("email")]
		public string Email { get; set;}
		
		

	}
}