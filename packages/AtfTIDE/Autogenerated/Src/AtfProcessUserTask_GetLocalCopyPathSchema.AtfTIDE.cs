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

	#region Class: AtfProcessUserTask_GetLocalCopyPath

	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "2998f9f15fc94c80afbf54830316d390", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "LocalCopyPath", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "2998f9f15fc94c80afbf54830316d390", CaptionResourceItem = "Parameters.LocalCopyPath.Caption", DescriptionResourceItem = "Parameters.LocalCopyPath.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_GetLocalCopyPath : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_GetLocalCopyPath(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("2998f9f1-5fc9-4c80-afbf-54830316d390");
		}

		#endregion

		#region Properties: Public

		public virtual Guid Repository {
			get;
			set;
		}

		public virtual string LocalCopyPath {
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
				if (!HasMapping("LocalCopyPath")) {
					writer.WriteValue("LocalCopyPath", LocalCopyPath, null);
				}
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
				case "LocalCopyPath":
					if (!UseFlowEngineMode) {
						break;
					}
					LocalCopyPath = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

