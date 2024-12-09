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

	#region Class: AtfProcessUserTask_DownloadPackages

	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "63ee21f56fab4a719eb3c7a5b44a63b8", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "63ee21f56fab4a719eb3c7a5b44a63b8", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Output", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "63ee21f56fab4a719eb3c7a5b44a63b8", CaptionResourceItem = "Parameters.Output.Caption", DescriptionResourceItem = "Parameters.Output.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "63ee21f56fab4a719eb3c7a5b44a63b8", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_DownloadPackages : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_DownloadPackages(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("63ee21f5-6fab-4a71-9eb3-c7a5b44a63b8");
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

		public virtual string Output {
			get;
			set;
		}

		public virtual Guid Repository {
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
			if (!HasMapping("ErrorMessage")) {
				writer.WriteValue("ErrorMessage", ErrorMessage, null);
			}
			if (!HasMapping("IsError")) {
				writer.WriteValue("IsError", IsError, false);
			}
			if (!HasMapping("Output")) {
				writer.WriteValue("Output", Output, null);
			}
			if (!HasMapping("Repository")) {
				writer.WriteValue("Repository", Repository, Guid.Empty);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "ErrorMessage":
					ErrorMessage = reader.GetStringValue();
				break;
				case "IsError":
					IsError = reader.GetBoolValue();
				break;
				case "Output":
					Output = reader.GetStringValue();
				break;
				case "Repository":
					Repository = reader.GetGuidValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

