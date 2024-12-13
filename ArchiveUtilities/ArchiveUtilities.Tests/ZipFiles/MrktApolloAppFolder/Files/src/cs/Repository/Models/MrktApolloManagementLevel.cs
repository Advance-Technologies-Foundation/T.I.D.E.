#pragma warning disable CS8618, // Non-nullable field is uninitialized.

using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("MrktApolloManagementLevel")]
	public class MrktApolloManagementLevel: BaseModel
	{

		/// <summary>
		/// Description visible to users
		/// </summary>
		[SchemaProperty("Description")]
		public string Description { get; set; }

		/// <summary>
		/// Apollo unique identifier
		/// </summary>
		[SchemaProperty("ApolloCode")]
		public string ApolloCode { get; set; }

	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
