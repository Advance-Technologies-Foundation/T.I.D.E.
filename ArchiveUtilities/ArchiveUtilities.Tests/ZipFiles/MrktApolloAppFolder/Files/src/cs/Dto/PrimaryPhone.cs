using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class PrimaryPhone
	{

		#region Properties: Public

		[JsonProperty("number")]
		public string Number { get; set; }

		[JsonProperty("sanitized_number")]
		public string SanitizedNumber { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		#endregion

	}
}