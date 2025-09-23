using AtfTIDE;
using AtfTIDE.Services;

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

	/// <exclude/>
	public partial class ATFProcessUserTask_InvokeLoadDataRequest
	{
		protected override bool InternalExecute(ProcessExecutingContext context) {
			IUiCommandService commandService = TideApp.Instance.GetRequiredService<IUiCommandService>();
			commandService.Invoke(CommandName, DataSourceName);
			return true;
		}
	}

	#endregion

}

