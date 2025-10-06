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
	using AtfTIDE.WebServices;

	#region Class: AtfProcessUserTask_InstallConsoleGit

	/// <exclude/>
	public partial class AtfProcessUserTask_InstallConsoleGit
	{
		protected override bool InternalExecute(ProcessExecutingContext context)
		{
			var tideService = new Tide();
			string installTideDestFolder = tideService.InstallConsoleGit();
			return !string.IsNullOrEmpty(installTideDestFolder);
		}
	}

	#endregion

}

