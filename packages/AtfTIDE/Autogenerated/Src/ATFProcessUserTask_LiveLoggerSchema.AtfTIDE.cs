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

	#region Class: ATFProcessUserTask_LiveLogger

	[DesignModeProperty(Name = "LogLevel", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "52d045707feb49b6af4a57b7b87aa253", CaptionResourceItem = "Parameters.LogLevel.Caption", DescriptionResourceItem = "Parameters.LogLevel.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "Message", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "52d045707feb49b6af4a57b7b87aa253", CaptionResourceItem = "Parameters.Message.Caption", DescriptionResourceItem = "Parameters.Message.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class ATFProcessUserTask_LiveLogger : ProcessUserTask
	{

		#region Constructors: Public

		public ATFProcessUserTask_LiveLogger(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("52d04570-7feb-49b6-af4a-57b7b87aa253");
		}

		#endregion

		#region Properties: Public

		public virtual string LogLevel {
			get;
			set;
		}

		public virtual string Message {
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
				if (!HasMapping("LogLevel")) {
					writer.WriteValue("LogLevel", LogLevel, null);
				}
			}
			if (!HasMapping("Message")) {
				writer.WriteValue("Message", Message, null);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "LogLevel":
					if (!UseFlowEngineMode) {
						break;
					}
					LogLevel = reader.GetStringValue();
				break;
				case "Message":
					Message = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

