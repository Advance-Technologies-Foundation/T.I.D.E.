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

	#region Class: AtfProcessUserTask_GitAddSome

	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "683313d5dd3e44288ccba7989644ff98", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "683313d5dd3e44288ccba7989644ff98", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Output", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "683313d5dd3e44288ccba7989644ff98", CaptionResourceItem = "Parameters.Output.Caption", DescriptionResourceItem = "Parameters.Output.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "683313d5dd3e44288ccba7989644ff98", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Files", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "683313d5dd3e44288ccba7989644ff98", CaptionResourceItem = "Parameters.Files.Caption", DescriptionResourceItem = "Parameters.Files.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_GitAddSome : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_GitAddSome(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("683313d5-dd3e-4428-8ccb-a7989644ff98");
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

		public virtual string Files {
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
			if (!HasMapping("Files")) {
				writer.WriteValue("Files", Files, null);
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
				case "Files":
					Files = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

