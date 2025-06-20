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

	#region Class: ATFProcessUserTask_ShowTerminal

	/// <exclude/>
	public partial class ATFProcessUserTask_ShowTerminal
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			IWebSocket ws = TideApp.Instance.GetRequiredService<IWebSocket>();
			ws.PostMessageToAll("ATFProcessUserTask_ShowTerminal", "ShowLogTerminal",string.Empty); 
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

