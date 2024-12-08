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

	#region Class: AtfProcessUserTask_CloneRepository

	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "RepositoryFolderPath", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.RepositoryFolderPath.Caption", DescriptionResourceItem = "Parameters.RepositoryFolderPath.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Output", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.Output.Caption", DescriptionResourceItem = "Parameters.Output.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_CloneRepository : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_CloneRepository(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("0988a3e0-cf0d-4d9d-9e24-3dcdd5606e3c");
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

		public virtual string RepositoryFolderPath {
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
				if (!HasMapping("RepositoryFolderPath")) {
					writer.WriteValue("RepositoryFolderPath", RepositoryFolderPath, null);
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
				case "RepositoryFolderPath":
					if (!UseFlowEngineMode) {
						break;
					}
					RepositoryFolderPath = reader.GetStringValue();
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

