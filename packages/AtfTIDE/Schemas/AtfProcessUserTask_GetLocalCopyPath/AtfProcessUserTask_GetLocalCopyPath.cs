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

	#region Class: AtfProcessUserTask_GetLocalCopyPath

	/// <exclude/>
	public partial class AtfProcessUserTask_GetLocalCopyPath
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			// IMPORTANT: When implementing long-running operations, it is crucial to
			// enable timely and responsive cancellation. To achieve this, ensure that your code is designed to
			// respond appropriately to cancellation requests using the context.CancellationToken mechanism.
			// For more detailed information and examples, please, refer to our documentation.
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			LocalCopyPath= HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString();
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

