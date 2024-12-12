using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class OrganizationEnrichmentDto
	{

		#region Properties: Public
		
		[JsonProperty("organization")]
		public Organization Organization { get; set; }

		#endregion

	}
}