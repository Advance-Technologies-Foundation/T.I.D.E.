using AtfTIDE;

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

	/// <exclude/>
	public partial class AtfProcessUserTask_GetComaSeparatedPathes
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "AtfRepository");
			esq.AddColumn("AtfName");
			
			var filterByUpdateAvailable = esq.CreateFilterWithParameters(FilterComparisonType.Equal, "AtfUpdateAvailable", true);
			esq.Filters.Add(filterByUpdateAvailable);
			
			var filterBySync = esq.CreateFilterWithParameters(FilterComparisonType.Equal, "AtfAutoSync", true);
			esq.Filters.Add(filterBySync);
			
			var collection = esq.GetEntityCollection(UserConnection);
			
			List<string> pathes = new List<string>();
			foreach (var entity in collection) {
				string name = entity.GetTypedColumnValue<string>("AtfName");
				if (!string.IsNullOrEmpty(name)) {
					var dirPath = HelperFunctions.GetRepositoryDirectory(name);
					pathes.Add(dirPath.FullName);
				}
			}
			Pathes = string.Join(", ", pathes);
			return true;
		}

		#endregion

		#region Methods: Public

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
		}

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override string GetExecutionData() {
			return string.Empty;
		}

		public override ProcessElementNotification GetNotificationData() {
			return base.GetNotificationData();
		}

		#endregion

	}

	#endregion

}

