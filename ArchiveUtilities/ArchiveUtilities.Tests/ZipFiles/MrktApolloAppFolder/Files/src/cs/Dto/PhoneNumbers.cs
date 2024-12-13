using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	internal class PhoneNumbers
	{
		[JsonProperty("raw_number")]
		public string RawNumber { get; set; }

		[JsonProperty("sanitized_number")]
		public string SanitizedNumber { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("position")]
		public int Position { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("dnc_status")]
		public object DncStatus { get; set; }

		[JsonProperty("dnc_other_info")]
		public object DncOtherInfo { get; set; }
		
		[JsonProperty("dialer_flags")]
		public object DialerFlags { get; set; }
	}
}