#pragma warning disable CS8618, // Non-nullable field is uninitialized.

using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("MrktApolloDepartment")]
	public class MrktApolloDepartment: BaseModel
	{

		/// <summary>
		/// Description visible to users
		/// </summary>
		[SchemaProperty("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Apollo unique identifier
		/// </summary>
		[SchemaProperty("ApolloCode")]
		public string ApolloCode { get; set; }
		
		
		[SchemaProperty("Department")]
		public Guid DepartmentId { get; set; }

		[LookupProperty("Department")]
		public virtual Department Department { get; set; }
	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
