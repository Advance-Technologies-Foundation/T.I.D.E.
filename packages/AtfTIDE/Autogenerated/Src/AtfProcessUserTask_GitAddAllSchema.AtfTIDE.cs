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

	#region Class: AtfProcessUserTask_GitAddAll

	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "c6bd9ac5873144cca92bbfb554fc62ef", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "c6bd9ac5873144cca92bbfb554fc62ef", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "c6bd9ac5873144cca92bbfb554fc62ef", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Output", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "c6bd9ac5873144cca92bbfb554fc62ef", CaptionResourceItem = "Parameters.Output.Caption", DescriptionResourceItem = "Parameters.Output.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_GitAddAll : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_GitAddAll(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("c6bd9ac5-8731-44cc-a92b-bfb554fc62ef");
		}

		#endregion

		#region Properties: Public

		public virtual string ErrorMessage {
			get;
			set;
		}

		public virtual bool IsError {
			get;
			set;
		}

		public virtual Guid Repository {
			get;
			set;
		}

		public virtual string Output {
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
				if (!HasMapping("Repository")) {
					writer.WriteValue("Repository", Repository, Guid.Empty);
				}
			}
			if (!HasMapping("Output")) {
				writer.WriteValue("Output", Output, null);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
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
				case "Repository":
					if (!UseFlowEngineMode) {
						break;
					}
					Repository = reader.GetGuidValue();
				break;
				case "Output":
					Output = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

