#pragma warning disable CS8618, // Non-nullable field is uninitialized.
using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("Country")]
	public class Country: BaseModel
	{
		
		[SchemaProperty("Name")]
		public string Name { get; set; }
		
		[SchemaProperty("Code")]
		public string Code { get; set; }

		/// <summary>
		/// ISO 3166-1 alpha-2
		/// </summary>
		[SchemaProperty("Alpha2Code")]
		public string Alpha2Code { get; set; }

	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
