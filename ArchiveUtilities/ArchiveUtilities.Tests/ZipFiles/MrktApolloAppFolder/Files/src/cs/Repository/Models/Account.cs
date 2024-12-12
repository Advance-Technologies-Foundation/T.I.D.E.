#pragma warning disable CS8618, // Non-nullable field is uninitialized.

using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("Account")]
	public class Account: BaseModel
	{

		/// <summary>
		/// Apollo unique identifier
		/// </summary>
		[SchemaProperty("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Apollo unique identifier
		/// </summary>
		[SchemaProperty("MrktApolloId")]
		public string MrktApolloId { get; set; }


		[SchemaProperty("Web")]
		public string Web { get; set; }

	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
