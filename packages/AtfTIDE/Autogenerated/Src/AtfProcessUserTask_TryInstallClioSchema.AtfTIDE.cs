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

	#region Class: AtfProcessUserTask_TryInstallClio

	[DesignModeProperty(Name = "ErrorMessage", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0fe5cc90943e498ab464ddaf1279bc81", CaptionResourceItem = "Parameters.ErrorMessage.Caption", DescriptionResourceItem = "Parameters.ErrorMessage.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "IsError", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0fe5cc90943e498ab464ddaf1279bc81", CaptionResourceItem = "Parameters.IsError.Caption", DescriptionResourceItem = "Parameters.IsError.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Output", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "0fe5cc90943e498ab464ddaf1279bc81", CaptionResourceItem = "Parameters.Output.Caption", DescriptionResourceItem = "Parameters.Output.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_TryInstallClio : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_TryInstallClio(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("0fe5cc90-943e-498a-b464-ddaf1279bc81");
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
			}
		}

		#endregion

	}

	#endregion

}

