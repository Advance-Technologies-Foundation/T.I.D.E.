namespace Terrasoft.Core.Process.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;

	#region Class: ATFProcessUserTask_GetBranches

	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0510a0cf480a4e3e9dbd0dcff795d52c", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0510a0cf480a4e3e9dbd0dcff795d52c", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0510a0cf480a4e3e9dbd0dcff795d52c", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Branches", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0510a0cf480a4e3e9dbd0dcff795d52c", CaptionResourceItem = "Parameters.Branches.Caption", DescriptionResourceItem = "Parameters.Branches.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class ATFProcessUserTask_GetBranches : ProcessUserTask
	{

		#region Constructors: Public

		public ATFProcessUserTask_GetBranches(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("0510a0cf-480a-4e3e-9dbd-0dcff795d52c");
		}

		#endregion

		#region Properties: Public

		public virtual Guid Repository {
			get;
			set;
		}

		public virtual string ErrorMessage {
			get;
			set;
		}

		public virtual bool IsError {
			get;
			set;
		}

		public virtual ICompositeObjectList<ICompositeObject> Branches {
			get;
			set;
		}

		#endregion

		#region Methods: Public

		public override void WritePropertiesData(DataWriter writer) {
			writer.WriteStartObject(Name);
			base.WritePropertiesData(writer);
			if (Status == Core.Process.ProcessStatus.Inactive) {
				writer.WriteFinishObject();
				return;
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("Repository")) {
					writer.WriteValue("Repository", Repository, Guid.Empty);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ErrorMessage")) {
					writer.WriteValue("ErrorMessage", ErrorMessage, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("IsError")) {
					writer.WriteValue("IsError", IsError, false);
				}
			}
			if (UseFlowEngineMode) {
				WriteSerializableObject<ICompositeObjectList<ICompositeObject>>(writer, "Branches", Branches);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "Repository":
					if (!UseFlowEngineMode) {
						break;
					}
					Repository = reader.GetGuidValue();
				break;
				case "ErrorMessage":
					if (!UseFlowEngineMode) {
						break;
					}
					ErrorMessage = reader.GetStringValue();
				break;
				case "IsError":
					if (!UseFlowEngineMode) {
						break;
					}
					IsError = reader.GetBoolValue();
				break;
				case "Branches":
					if (!UseFlowEngineMode) {
						break;
					}
					Branches = ReadSerializableObject<ICompositeObjectList<ICompositeObject>>(reader);
				break;
			}
		}

		#endregion

	}

	#endregion

}

