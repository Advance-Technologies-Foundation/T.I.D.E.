#pragma warning disable CS8618, // Non-nullable field is uninitialized.

using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("Department")]
	public class Department: BaseModel
	{

		[SchemaProperty("CreatedOn")]
		public DateTime CreatedOn { get; set; }

		[SchemaProperty("CreatedBy")]
		public Guid CreatedById { get; set; }

		[LookupProperty("CreatedBy")]
		public virtual Contact CreatedBy { get; set; }

		[SchemaProperty("ModifiedOn")]
		public DateTime ModifiedOn { get; set; }

		[SchemaProperty("ModifiedBy")]
		public Guid ModifiedById { get; set; }

		[LookupProperty("ModifiedBy")]
		public virtual Contact ModifiedBy { get; set; }

		[SchemaProperty("Name")]
		public string Name { get; set; }

		[SchemaProperty("Description")]
		public string Description { get; set; }

		[SchemaProperty("ProcessListeners")]
		public int ProcessListeners { get; set; }

	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
