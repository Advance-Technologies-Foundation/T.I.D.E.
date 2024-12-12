using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	public class ContactEmail
	{

		[JsonProperty("email")]
		public string Email { get; set; }
		
		[JsonProperty("email_md5")]
		public string EmailMd5 { get; set; }
		
		[JsonProperty("email_sha256")]
		public string EmailSha256 { get; set; }
		
		[JsonProperty("email_status")]
		public string EmailStatus { get; set; }
		
		[JsonProperty("email_source")]
		public string EmailSource { get; set; }
		
		[JsonProperty("extrapolated_email_confidence")]
		public object ExtrapolatedEmailConfidence { get; set; }
		
		[JsonProperty("position")]
		public int Position { get; set; }
		
		[JsonProperty("email_from_customer")]
		public object EmailFromCustomer { get; set; }
		
		[JsonProperty("free_domain")]
		public bool FreeDomain { get; set; }

	}
}



