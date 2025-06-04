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

	#region Class: AtfProcessUserTask_GetComaSeparatedPathes

	[DesignModeProperty(Name = "Pathes", Group = "", ValuesProvider = "ProcessSchemaParameterValueProvider", Editor="xtype=processschemaparametervalueedit;dataProvider=processschemaparametervalueprovider", ResourceManager = "b82ef6547a4f4f0fabbb9627e30587f8", CaptionResourceItem = "Parameters.Pathes.Caption", DescriptionResourceItem = "Parameters.Pathes.Caption", UseSolutionStorage = true)]
	/// <exclude/>
	public partial class AtfProcessUserTask_GetComaSeparatedPathes : ProcessUserTask
	{

		#region Constructors: Public

		public AtfProcessUserTask_GetComaSeparatedPathes(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("b82ef654-7a4f-4f0f-abbb-9627e30587f8");
		}

		#endregion

		#region Properties: Public

		public virtual string Pathes {
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
			if (!HasMapping("Pathes")) {
				writer.WriteValue("Pathes", Pathes, null);
			}
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
				case "Pathes":
					Pathes = reader.GetStringValue();
				break;
			}
		}

		#endregion

	}

	#endregion

}

