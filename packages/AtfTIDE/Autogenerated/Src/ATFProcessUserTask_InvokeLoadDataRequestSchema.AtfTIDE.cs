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

	#region Class: ATFProcessUserTask_InvokeLoadDataRequest

	[DesignModeProperty(Name = "CommandName", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "86ed7e85b7ca432ab75edd54dbbb25c6", CaptionResourceItem = "Parameters.CommandName.Caption", DescriptionResourceItem = "Parameters.CommandName.Caption", UseSolutionStorage = true)]
	[DesignModeProperty(Name = "DataSourceName", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "86ed7e85b7ca432ab75edd54dbbb25c6", CaptionResourceItem = "Parameters.DataSourceName.Caption", DescriptionResourceItem = "Parameters.DataSourceName.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class ATFProcessUserTask_InvokeLoadDataRequest : ProcessUserTask
	{

		#region Constructors: Public

		public ATFProcessUserTask_InvokeLoadDataRequest(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("86ed7e85-b7ca-432a-b75e-dd54dbbb25c6");
		}

		#endregion

		#region Properties: Public

		public virtual string CommandName {
			get;
			set;
		}

		public virtual string DataSourceName {
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
			if (!HasMapping("CommandName")) {
				writer.WriteValue("CommandName", CommandName, null);
			}
			if (!HasMapping("DataSourceName")) {
				writer.WriteValue("DataSourceName", DataSourceName, null);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "CommandName":
					CommandName = reader.GetStringValue();
				break;
				case "DataSourceName":
					DataSourceName = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

