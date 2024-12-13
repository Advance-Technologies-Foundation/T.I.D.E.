#pragma warning disable CS8618, // Non-nullable field is uninitialized.

using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("SysSettingsValue")]
	public class SysSettingsValue: BaseModel
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

		[SchemaProperty("SysSettings")]
		public Guid SysSettingsId { get; set; }

		[LookupProperty("SysSettings")]
		public virtual SysSettings SysSettings { get; set; }
		
		[SchemaProperty("IsDef")]
		public bool IsDef { get; set; }

		[SchemaProperty("TextValue")]
		public string TextValue { get; set; }

		[SchemaProperty("IntegerValue")]
		public int IntegerValue { get; set; }

		[SchemaProperty("FloatValue")]
		public decimal FloatValue { get; set; }

		[SchemaProperty("BooleanValue")]
		public bool BooleanValue { get; set; }

		[SchemaProperty("DateTimeValue")]
		public DateTime DateTimeValue { get; set; }

		[SchemaProperty("GuidValue")]
		public Guid GuidValue { get; set; }

		[SchemaProperty("BinaryValue")]
		public byte[] BinaryValue { get; set; }

		[SchemaProperty("Position")]
		public int Position { get; set; }

		[SchemaProperty("ProcessListeners")]
		public int ProcessListeners { get; set; }

	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
