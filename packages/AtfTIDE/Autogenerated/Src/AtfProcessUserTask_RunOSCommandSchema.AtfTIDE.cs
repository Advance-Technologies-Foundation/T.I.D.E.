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

	#region Class: AtfProcessUserTask_RunOSCommand

	[DesignModeProperty(Name = "FileName", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ad6a7f037b144d0696c0bf1c01c1d8f8", CaptionResourceItem = "Parameters.FileName.Caption", DescriptionResourceItem = "Parameters.FileName.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Arguments", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ad6a7f037b144d0696c0bf1c01c1d8f8", CaptionResourceItem = "Parameters.Arguments.Caption", DescriptionResourceItem = "Parameters.Arguments.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "WorkingDirectory", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ad6a7f037b144d0696c0bf1c01c1d8f8", CaptionResourceItem = "Parameters.WorkingDirectory.Caption", DescriptionResourceItem = "Parameters.WorkingDirectory.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ad6a7f037b144d0696c0bf1c01c1d8f8", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "ad6a7f037b144d0696c0bf1c01c1d8f8", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_RunOSCommand : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_RunOSCommand(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("ad6a7f03-7b14-4d06-96c0-bf1c01c1d8f8");
		}

		#endregion

		#region Properties: Public

		public virtual string FileName {
			get;
			set;
		}

		public virtual string Arguments {
			get;
			set;
		}

		public virtual string WorkingDirectory {
			get;
			set;
		}

		public virtual bool IsError {
			get;
			set;
		}

		public virtual string ErrorMessage {
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
				if (!HasMapping("FileName")) {
					writer.WriteValue("FileName", FileName, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("Arguments")) {
					writer.WriteValue("Arguments", Arguments, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("WorkingDirectory")) {
					writer.WriteValue("WorkingDirectory", WorkingDirectory, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("IsError")) {
					writer.WriteValue("IsError", IsError, false);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("ErrorMessage")) {
					writer.WriteValue("ErrorMessage", ErrorMessage, null);
				}
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "FileName":
					if (!UseFlowEngineMode) {
						break;
					}
					FileName = reader.GetStringValue();
				break;
				case "Arguments":
					if (!UseFlowEngineMode) {
						break;
					}
					Arguments = reader.GetStringValue();
				break;
				case "WorkingDirectory":
					if (!UseFlowEngineMode) {
						break;
					}
					WorkingDirectory = reader.GetStringValue();
				break;
				case "IsError":
					if (!UseFlowEngineMode) {
						break;
					}
					IsError = reader.GetBoolValue();
				break;
				case "ErrorMessage":
					if (!UseFlowEngineMode) {
						break;
					}
					ErrorMessage = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

