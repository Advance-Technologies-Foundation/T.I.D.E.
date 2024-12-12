using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	internal class MixedCompanySearchDto : BaseApolloRequest
	{

		#region Constructors: Public

		public MixedCompanySearchDto(int page, int perPage) : base(page, perPage){ }

		#endregion

		#region Properties: Public

		[JsonProperty("prospected_by_current_team")]
		public bool IncludeExisting { get; set; }

		[JsonProperty("q_organization_name")]
		public string SearchTerm { get; set; }

		#endregion

	}
}