using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class Breadcrumb
	{

		#region Properties: Public

		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("signal_field_name")]
		public string SignalFieldName { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		#endregion

	}
}