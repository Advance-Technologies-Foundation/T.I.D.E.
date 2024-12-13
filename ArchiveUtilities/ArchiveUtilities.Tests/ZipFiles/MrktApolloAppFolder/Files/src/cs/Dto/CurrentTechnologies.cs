using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class CurrentTechnologies
	{
		#region Properties: Public

		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("uid")]
		public string Uid { get; set; }

		#endregion
	}
}