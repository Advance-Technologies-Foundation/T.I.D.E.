using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class Pagination
	{

		#region Properties: Public

		[JsonProperty("page")]
		public int Page { get; set; }

		[JsonProperty("per_page")]
		public int PerPage { get; set; }

		[JsonProperty("total_entries")]
		public int TotalEntries { get; set; }

		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }

		#endregion

	}
}