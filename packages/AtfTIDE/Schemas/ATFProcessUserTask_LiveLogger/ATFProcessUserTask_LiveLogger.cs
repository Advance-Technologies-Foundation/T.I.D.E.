using AtfTIDE;
using AtfTIDE.Logging;

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

	/// <exclude/>
	public partial class ATFProcessUserTask_LiveLogger
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			ILiveLogger logger = TideApp.Instance.GetRequiredService<ILiveLogger>();
			if(LogLevel.ToUpperInvariant().Equals("INF", StringComparison.InvariantCulture)){
				logger.LogInfo(Message);	
			}
			else if(LogLevel.ToUpperInvariant().Equals("WAR", StringComparison.InvariantCulture)){
				logger.LogWarn(Message);	
			}
			else if(LogLevel.ToUpperInvariant().Equals("ERR", StringComparison.InvariantCulture)){
				logger.LogError(Message);	
			}
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

