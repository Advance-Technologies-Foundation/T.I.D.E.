using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[JsonObject]
	[ExcludeFromCodeCoverage]
	internal class PeopleMatchResponse
	{
		[JsonProperty("person")]
		public Person Person { get; set; }
	}
}