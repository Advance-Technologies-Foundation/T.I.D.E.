using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class DepartmentalHeadCount
	{
		#region Properties: Public

		[JsonProperty("accounting")]
		public int Accounting { get; set; }

		[JsonProperty("administrative")]
		public int Administrative { get; set; }

		[JsonProperty("arts_and_design")]
		public int ArtsAndDesign { get; set; }

		[JsonProperty("business_development")]
		public int BusinessDevelopment { get; set; }

		[JsonProperty("consulting")]
		public int Consulting { get; set; }

		[JsonProperty("data_science")]
		public int DataScience { get; set; }

		[JsonProperty("education")]
		public int Education { get; set; }

		[JsonProperty("engineering")]
		public int Engineering { get; set; }

		[JsonProperty("entrepreneurship")]
		public int Entrepreneurship { get; set; }

		[JsonProperty("finance")]
		public int Finance { get; set; }

		[JsonProperty("human_resources")]
		public int HumanResources { get; set; }

		[JsonProperty("information_technology")]
		public int InformationTechnology { get; set; }

		[JsonProperty("legal")]
		public int Legal { get; set; }

		[JsonProperty("marketing")]
		public int Marketing { get; set; }

		[JsonProperty("media_and_commmunication")]
		public int MediaAndCommunication { get; set; }

		[JsonProperty("operations")]
		public int Operations { get; set; }

		[JsonProperty("product_management")]
		public int ProductManagement { get; set; }

		[JsonProperty("sales")]
		public int Sales { get; set; }

		[JsonProperty("support")]
		public int Support { get; set; }

		#endregion
	}
}