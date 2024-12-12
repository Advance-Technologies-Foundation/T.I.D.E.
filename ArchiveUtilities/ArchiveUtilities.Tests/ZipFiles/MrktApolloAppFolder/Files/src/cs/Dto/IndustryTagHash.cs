using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class IndustryTagHash
	{

		#region Properties: Public

		[JsonProperty("computer_software")]
		public string ComputerSoftware { get; set; }

		[JsonProperty("information_technology___services")]
		public string InformationTechnologyServices { get; set; }

		#endregion

	}
}