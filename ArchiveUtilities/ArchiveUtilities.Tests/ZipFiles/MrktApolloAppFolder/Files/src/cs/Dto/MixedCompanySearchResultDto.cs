using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class MixedCompanySearchResultDto
	{

		#region Properties: Public

		[JsonProperty("accounts")]
		public IEnumerable<Company> Accounts { get; set; }

		[JsonProperty("breadcrumbs")]
		public IEnumerable<Breadcrumb> Breadcrumbs { get; set; }

		[JsonProperty("derived_params")]
		public DerivedParams DerivedParams { get; set; }

		[JsonProperty("disable_eu_prospecting")]
		public bool DisableEuProspecting { get; set; }

		[JsonProperty("model_ids")]
		public IEnumerable<string> ModelIds { get; set; }

		[JsonProperty("num_fetch_result")]
		public object NumFetchResult { get; set; }

		[JsonProperty("organizations")]
		public IEnumerable<Company> Organizations { get; set; }

		[JsonProperty("pagination")]
		public Pagination Pagination { get; set; }

		[JsonProperty("partial_results_limit")]
		public int PartialResultsLimit { get; set; }

		[JsonProperty("partial_results_only")]
		public bool PartialResultsOnly { get; set; }

		#endregion

	}
}