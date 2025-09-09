using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using AtfTIDE.ClioInstaller;
using Common.Logging;
using ErrorOr;
using Terrasoft.Core;
using Terrasoft.Core.Configuration;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;

namespace AtfTIDE
{
	public class AppEventListener : IAppEventListener {

		private const string ClioNugetPackageName = "clio";
		private Thread _sysSchemaMonitorThread;
		
		/// <summary>
		/// Handles application startup: starts schema monitor, ensures Clio is installed,
		/// checks for Clio and AtfTide updates, and updates related system settings.
		/// </summary>
		/// /// <remarks>
		/// Updates system settings:
		/// <list type="bullet">
		/// <item><description><c>TideConsts.SysSettingClioPath</c> - absolute path to installed <c>clio.dll</c>.</description></item>
		/// <item><description><c>AtfTideUpdateAvailable</c> - whether a newer NuGet version of AtfTide exists.</description></item>
		/// <item><description><c>AtfClioUpdateAvailable</c> - whether a newer NuGet version of Clio exists.</description></item>
		/// </list>
		/// </remarks>
		public void OnAppStart(AppEventContext context) {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			_sysSchemaMonitorThread = new Thread(SysSchemaMonitor.Start);
			_sysSchemaMonitorThread.Start();

			if (IsClioInstalled(userConnection)) {
				CheckForClioUpdates(userConnection);
			} else {
				LogManager.GetLogger(TideConsts.LoggerName).ErrorFormat(CultureInfo.InvariantCulture, "Clio is not installed or the path does not match.");
				IErrorOr<Success> result = TideApp.Instance.InstallerApp.InstallClio();
				if(!result.IsError) {
					string clioFilePath = HelperFunctions.GetClioFilePath();
					SysSettings.SetValue(userConnection, TideConsts.SysSettingClioPath, clioFilePath);
				}
			}

			ErrorOr<string> maybeMaxTideVersion = TideApp.Instance.GetRequiredService<INugetClient>()
					.GetMaxVersionAsync("AtfTide")
					.GetAwaiter().GetResult();

			if (maybeMaxTideVersion.IsError) {
				LogManager.GetLogger(TideConsts.LoggerName).ErrorFormat(CultureInfo.InvariantCulture, $"{maybeMaxTideVersion.FirstError.Code} - {maybeMaxTideVersion.FirstError.Description}");
				return;
			}
			string nugetMaxTideVersion = maybeMaxTideVersion.Value;

			EntitySchemaQuery esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "SysPackage");
			esq.AddAllSchemaColumns();

			IEntitySchemaQueryFilterItem nameFilter =
				esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Name", "AtfTide");
			esq.Filters.Add(nameFilter);

			EntityCollection collection = esq.GetEntityCollection(userConnection);
			if (collection.Count == 1) {
				string version = collection[0].GetTypedColumnValue<string>("Version");

				Version installedV = Version.Parse(version);
				Version nugetV = Version.Parse(nugetMaxTideVersion);
				bool updateAvailable = nugetV.CompareTo(installedV) == 1;

				LogManager.GetLogger(TideConsts.LoggerName)
						.InfoFormat(CultureInfo.InvariantCulture,  $"Updating SysSetting AtfTideUpdateAvailable to: {updateAvailable}, AtfTideVersion: {version}, NugetMaxTideVersion: {nugetMaxTideVersion}");
				SysSettings.SetDefValue(userConnection, "AtfTideUpdateAvailable", updateAvailable);
			}
		}

		/// <summary>
		/// Checks whether Clio is considered installed by verifying that:
		/// 1) The expected Clio directory exists.
		/// 2) A file named 'clio.dll' exists within that directory (searched recursively).
		/// 3) The full path of one of the discovered 'clio.dll' files matches the stored system setting.
		/// </summary>
		/// <param name="userConnection">Active user connection used to read system settings.</param>
		/// <returns>
		/// true if a 'clio.dll' file exists under the Clio directory and its full path matches the value
		/// stored in the system setting defined by <c>TideConsts.SysSettingClioPath</c>; otherwise false.
		/// </returns>
		private bool IsClioInstalled(UserConnection userConnection) {
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			if (!clioDir.Exists) {
				return false;
			}
			
			string clioFilePathSetting = SysSettings.GetValue<string>(userConnection, TideConsts.SysSettingClioPath, string.Empty);
			FileInfo[] clioFiles = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
			return clioFiles.Any(file => file.FullName.Equals(clioFilePathSetting, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Checks whether a newer Clio NuGet version exists than the installed <c>clio.dll</c>.
		/// Updates system setting <c>AtfClioUpdateAvailable</c> to reflect availability.
		/// Returns silently if the directory or file is missing, or versions cannot be determined.
		/// </summary>
		private void CheckForClioUpdates(UserConnection userConnection) {
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			if (!clioDir.Exists) {
				return;
			}

			FileInfo[] clioFiles = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
			if (clioFiles.Length == 0) {
				return;
			}

			FileInfo clioFile = clioFiles.First();
			Version installedVersion = GetInstalledClioVersion(clioFile);
			Version latestVersion = GetLatestClioVersion();

			if (installedVersion != null && latestVersion != null && latestVersion > installedVersion) {
				SysSettings.SetValue(userConnection, "AtfClioUpdateAvailable", true);
			} else {
				SysSettings.SetValue(userConnection, "AtfClioUpdateAvailable", false);
			}
		}

		/// <summary>
		/// Gets the installed Clio assembly version by reading the file version information of the supplied clio file.
		/// </summary>
		/// <param name="clioFile">File info referencing the expected clio.dll.</param>
		/// <returns>
		/// The parsed Version of the file, or null if the file info is null or the file does not exist.
		/// </returns>
		/// <remarks>
		/// Relies on FileVersionInfo; any malformed version string will surface as an exception to the caller.
		/// </remarks>
		private Version GetInstalledClioVersion(FileInfo clioFile) {
			if (clioFile == null || !clioFile.Exists) {
				return null;
			}
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(clioFile.FullName);
			return new Version(versionInfo.FileVersion);
		}

		/// <summary>
		/// Gets the latest Clio version from the NuGet server.
		/// </summary>
		/// <returns>
		/// The parsed Version of the latest version, or null if the version could not be retrieved.
		/// </returns>
		/// <remarks>
		/// Relies on the NuGet API; any malformed version string will surface as an exception to the caller.
		/// </remarks>
		private Version GetLatestClioVersion() {
			try {
				INugetClient nugetClient = TideApp.Instance.GetRequiredService<INugetClient>();
				ErrorOr<string> maybeMaxClioVersion = nugetClient.GetMaxVersionAsync(ClioNugetPackageName).GetAwaiter().GetResult();

				if (maybeMaxClioVersion.IsError) {
					LogManager.GetLogger(TideConsts.LoggerName).ErrorFormat(CultureInfo.InvariantCulture, $"{maybeMaxClioVersion.FirstError.Code} - {maybeMaxClioVersion.FirstError.Description}");
					return null;
				}

				string maxClioVersion = maybeMaxClioVersion.Value;
				return new Version(maxClioVersion);
			} catch (Exception ex) {
				// Log error but don't throw - we want the process to continue
				// Just return null to indicate we couldn't get the version
				LogManager.GetLogger(TideConsts.LoggerName).ErrorFormat(CultureInfo.InvariantCulture, $"Error fetching Clio version: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Handles application end: stops the schema monitor thread safely.
		/// </summary>
		public void OnAppEnd(AppEventContext context){
			_sysSchemaMonitorThread = new Thread(SysSchemaMonitor.Stop);
			_sysSchemaMonitorThread.Join();
			_sysSchemaMonitorThread = null;
			
			return;
		}

		public void OnSessionStart(AppEventContext context){
			return;
		}

		public void OnSessionEnd(AppEventContext context){
			return;
		}

	}
}
