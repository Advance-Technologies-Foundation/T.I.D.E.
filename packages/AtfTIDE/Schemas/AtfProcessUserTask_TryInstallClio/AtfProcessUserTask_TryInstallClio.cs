using System.Diagnostics;
using System.IO;
using System.Linq;
using AtfTIDE;
using AtfTIDE.ClioInstaller;
using ErrorOr;
using Terrasoft.Core.Factories;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Reflection;

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

	#region Class: AtfProcessUserTask_TryInstallClio

	/// <exclude/>
	public partial class AtfProcessUserTask_TryInstallClio
	{
		private const string ClioNugetPackageName = "creatio.clio";

		#region Methods: Protected

		protected Version GetInstalledClioVersion(FileInfo clioFile)
		{
			if (clioFile == null || !clioFile.Exists)
			{
				return null;
			}
			
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(clioFile.FullName);
			return new Version(versionInfo.FileVersion);
		}
		
		protected Version GetLatestClioVersion()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Add("User-Agent", "TIDE-ClioVersionChecker");
					string url = $"https://api.nuget.org/v3/registration5-semver1/{ClioNugetPackageName}/index.json";
					
					HttpResponseMessage response = client.GetAsync(url).Result;
					if (response.IsSuccessStatusCode)
					{
						string json = response.Content.ReadAsStringAsync().Result;
						using (JsonDocument doc = JsonDocument.Parse(json))
						{
							JsonElement root = doc.RootElement;
							JsonElement items = root.GetProperty("items")[0];
							JsonElement items2 = items.GetProperty("items");
							
							// Get the latest version
							JsonElement latestPackage = items2[items2.GetArrayLength() - 1];
							string versionString = latestPackage.GetProperty("catalogEntry").GetProperty("version").GetString();
							
							return new Version(versionString);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Log error but don't throw - we want the process to continue
				// Just return null to indicate we couldn't get the version
			}
			
			return null;
		}
		
		protected void CheckForClioUpdates(DirectoryInfo clioDir)
		{
			if (!clioDir.Exists)
			{
				return;
			}
			
			FileInfo[] clioFiles = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
			if (clioFiles.Length == 0)
			{
				return;
			}
			
			FileInfo clioFile = clioFiles.First();
			Version installedVersion = GetInstalledClioVersion(clioFile);
			Version latestVersion = GetLatestClioVersion();
			
			if (installedVersion != null && latestVersion != null && latestVersion > installedVersion)
			{
				// Update is available, set system setting
				SysSettings.SetValue(UserConnection, "AtfClioUpdfateAvailable", true);
			}
			else
			{
				// No update available, set to false
				SysSettings.SetValue(UserConnection, "AtfClioUpdfateAvailable", false);
			}
		}

		protected bool IsClioInstalled(DirectoryInfo clioDir) {
			if (!clioDir.Exists) {
				return false;
			}
			
			FileInfo[] clioFiles = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
			return clioFiles.Length > 0;
		}

		protected override bool InternalExecute(ProcessExecutingContext context) {
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			
			// Check if Clio is already installed
			if (IsClioInstalled(clioDir)) {
				// Clio is already installed, no need to reinstall
				FileInfo[] clioFilePath = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
				SysSettings.SetValue(UserConnection, "AtfClioFilePath", clioFilePath.First().FullName);
				
				// Check for updates synchronously
				CheckForClioUpdates(clioDir);
				
				return true;
			}
			
			// Clio is not installed or directory doesn't exist, proceed with installation
			if (!clioDir.Exists) {
				clioDir.Create();
			} else {
				clioDir.Delete(true);
				clioDir.Create();
			}
			
			IErrorOr<Success> result = TideApp.Create().InstallerApp.InstallClio();
			if(result.IsError) {
				ErrorMessage = $"{result.Errors.FirstOrDefault().Code} - {result.Errors.FirstOrDefault().Description}";
				IsError = true;
				
			} else {
				FileInfo[] clioFilePath = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
				SysSettings.SetValue(UserConnection, "AtfClioFilePath", clioFilePath.First().FullName);
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

