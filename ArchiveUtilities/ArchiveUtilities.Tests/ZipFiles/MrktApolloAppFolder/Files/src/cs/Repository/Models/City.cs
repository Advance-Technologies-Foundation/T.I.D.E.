#pragma warning disable CS8618, // Non-nullable field is uninitialized.
using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{
	[ExcludeFromCodeCoverage]
	[Schema("City")]
	public class City: BaseModel
	{
		
		[SchemaProperty("Name")]
		public string Name { get; set; }

		[SchemaProperty("Description")]
		public string Description { get; set; }

		[SchemaProperty("Country")]
		public Guid CountryId { get; set; }

		[LookupProperty("Country")]
		public virtual Country Country { get; set; }

		[SchemaProperty("Region")]
		public Guid RegionId { get; set; }

		[LookupProperty("Region")]
		public virtual Region Region { get; set; }
		
	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
