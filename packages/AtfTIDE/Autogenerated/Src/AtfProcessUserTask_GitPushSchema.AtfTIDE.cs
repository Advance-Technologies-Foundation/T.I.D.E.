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

	#region Class: AtfProcessUserTask_GitPush

	[DesignModeProperty(Name = "Repository", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "15d4abc17e0d4aa4a3365f16b73b8be1", CaptionResourceItem = "Parameters.Repository.Caption", DescriptionResourceItem = "Parameters.Repository.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "15d4abc17e0d4aa4a3365f16b73b8be1", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "15d4abc17e0d4aa4a3365f16b73b8be1", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Output", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "15d4abc17e0d4aa4a3365f16b73b8be1", CaptionResourceItem = "Parameters.Output.Caption", DescriptionResourceItem = "Parameters.Output.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_GitPush : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_GitPush(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("15d4abc1-7e0d-4aa4-a336-5f16b73b8be1");
		}

		#endregion

		#region Properties: Public

		public virtual Guid Repository {
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
			if (!HasMapping("Repository")) {
				writer.WriteValue("Repository", Repository, Guid.Empty);
			}
			if (!HasMapping("IsError")) {
				writer.WriteValue("IsError", IsError, false);
			}
			if (!HasMapping("ErrorMessage")) {
				writer.WriteValue("ErrorMessage", ErrorMessage, null);
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
				case "Repository":
					Repository = reader.GetGuidValue();
				break;
				case "IsError":
					IsError = reader.GetBoolValue();
				break;
				case "ErrorMessage":
					ErrorMessage = reader.GetStringValue();
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

