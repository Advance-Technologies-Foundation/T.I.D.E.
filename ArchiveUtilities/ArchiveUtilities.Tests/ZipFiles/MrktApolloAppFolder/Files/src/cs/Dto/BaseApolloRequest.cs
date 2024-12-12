using Newtonsoft.Json;
using Terrasoft.Common.Json;

namespace MrktApolloApp.Dto
{
	public abstract class BaseApolloRequest
	{

		#region Constructors: Protected

		protected BaseApolloRequest(int page, int perPage){
			Page = page;
			PerPage = perPage;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// requested page
		/// </summary>
		[JsonProperty("page")]
		public int Page { get; set; }

		/// <summary>
		/// Number of records per page
		/// </summary>
		[JsonProperty("per_page")]
		public int PerPage { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Serialize object to json string
		/// </summary>
		/// <returns>JSON</returns>
		public override string ToString() =>
			Json.FormatJsonString(JsonConvert.SerializeObject(this), Formatting.Indented);

		#endregion

	}
}