
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class PeopleSearchResponse
	{

		[JsonProperty("people")]
		public IEnumerable<Person> People { get; set; }
		
		[JsonProperty("pagination")]
		public Pagination Pagination { get; set; }
		
		[JsonProperty("contacts")]
		public IEnumerable<Contact> Contacts { get; set; }
		
	}
}
