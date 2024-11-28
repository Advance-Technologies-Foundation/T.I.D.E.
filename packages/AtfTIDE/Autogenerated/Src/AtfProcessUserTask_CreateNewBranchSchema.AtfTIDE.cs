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

	#region Class: AtfProcessUserTask_CreateNewBranch

	[DesignModeProperty(Name = "RepositoryUrl", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.RepositoryUrl.Caption", DescriptionResourceItem = "Parameters.RepositoryUrl.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "AccessToken", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.AccessToken.Caption", DescriptionResourceItem = "Parameters.AccessToken.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "BranchName", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.BranchName.Caption", DescriptionResourceItem = "Parameters.BranchName.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0988a3e0cf0d4d9d9e243dcdd5606e3c", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_CreateNewBranch : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_CreateNewBranch(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("0988a3e0-cf0d-4d9d-9e24-3dcdd5606e3c");
		}

		#endregion

		#region Properties: Public

		public virtual string RepositoryUrl {
			get;
			set;
		}

		public virtual string AccessToken {
			get;
			set;
		}

		public virtual string BranchName {
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
				if (!HasMapping("RepositoryUrl")) {
					writer.WriteValue("RepositoryUrl", RepositoryUrl, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("AccessToken")) {
					writer.WriteValue("AccessToken", AccessToken, null);
				}
			}
			if (UseFlowEngineMode) {
				if (!HasMapping("BranchName")) {
					writer.WriteValue("BranchName", BranchName, null);
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
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "RepositoryUrl":
					if (!UseFlowEngineMode) {
						break;
					}
					RepositoryUrl = reader.GetStringValue();
				break;
				case "AccessToken":
					if (!UseFlowEngineMode) {
						break;
					}
					AccessToken = reader.GetStringValue();
				break;
				case "BranchName":
					if (!UseFlowEngineMode) {
						break;
					}
					BranchName = reader.GetStringValue();
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
			}
		}

		#endregion

	}

	#endregion

}

