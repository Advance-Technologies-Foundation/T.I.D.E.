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

	#region Class: ATFProcessUserTask_ShowTerminal

	/// <exclude/>
	public partial class ATFProcessUserTask_ShowTerminal : ProcessUserTask
	{

		#region Constructors: Public

		public ATFProcessUserTask_ShowTerminal(UserConnection userConnection)
			: base(userConnection) {
			SchemaUId = new Guid("6fe450a0-55e1-43ac-a657-cb52987c233d");
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
			writer.WriteFinishObject();
		}

		#endregion

		#region Methods: Protected

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			switch (reader.CurrentName) {
			}
		}

		#endregion

	}

	#endregion

}

