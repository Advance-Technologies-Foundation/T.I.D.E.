using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;


namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class FundingEvents
	{

		#region Properties: Public

		[JsonProperty("amount")]
		public string Amount { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("date")]
		public string Date { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("investors")]
		public string Investors { get; set; }

		[JsonProperty("news_url")]
		public object NewsUrl { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		#endregion

	}
}