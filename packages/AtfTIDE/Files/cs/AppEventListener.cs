using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Reflection;
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

		public void OnAppStart(AppEventContext context) {
			var userConnection = ClassFactory.Get<UserConnection>();

			if (IsClioInstalled(userConnection)) {
				CheckForClioUpdates(userConnection);
			} else {
				LogManager.GetLogger("AtfTide").ErrorFormat(CultureInfo.InvariantCulture, "Clio is not installed or the path does not match.");
			}

			var maybeMaxTideVersion = TideApp.Instance.GetRequiredService<INugetClient>()
					.GetMaxVersionAsync("AtfTide")
					.GetAwaiter().GetResult();

			if (maybeMaxTideVersion.IsError) {
				LogManager.GetLogger("AtfTide").ErrorFormat(CultureInfo.InvariantCulture, $"{maybeMaxTideVersion.FirstError.Code} - {maybeMaxTideVersion.FirstError.Description}");
				return;
			}
			string nugetMaxTideVersion = maybeMaxTideVersion.Value;

			EntitySchemaQuery esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "SysPackage");
			esq.AddAllSchemaColumns();

			IEntitySchemaQueryFilterItem nameFilter =
				esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Name", "AtfTide");
			esq.Filters.Add(nameFilter);

			var collection = esq.GetEntityCollection(userConnection);
			if (collection.Count == 1) {
				var version = collection[0].GetTypedColumnValue<string>("Version");

				var installedV = Version.Parse(version);
				var nugetV = Version.Parse(nugetMaxTideVersion);
				bool updateAvailable = nugetV.CompareTo(installedV) == 1;

				LogManager.GetLogger("AtfTide")
						.InfoFormat(CultureInfo.InvariantCulture,  $"Updating SysSetting AtfTideUpdateAvailable to: {updateAvailable}, AtfTideVersion: {version}, NugetMaxTideVersion: {nugetMaxTideVersion}");
				SysSettings.SetDefValue(userConnection, "AtfTideUpdateAvailable", updateAvailable);
			}
		}

		private bool IsClioInstalled(UserConnection userConnection) {
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			if (!clioDir.Exists) {
				return false;
			}

			string clioFilePathSetting = SysSettings.GetValue<string>(userConnection, "AtfClioFilePath");
			FileInfo[] clioFiles = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
			return clioFiles.Any(file => file.FullName.Equals(clioFilePathSetting, StringComparison.OrdinalIgnoreCase));
		}

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

		private Version GetInstalledClioVersion(FileInfo clioFile) {
			if (clioFile == null || !clioFile.Exists) {
				return null;
			}

			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(clioFile.FullName);
			return new Version(versionInfo.FileVersion);
		}

		private Version GetLatestClioVersion() {
			try {
				using (HttpClient client = new HttpClient()) {
					client.DefaultRequestHeaders.Add("User-Agent", "TIDE-ClioVersionChecker");
					string url = $"https://api.nuget.org/v3/registration5-semver1/{ClioNugetPackageName}/index.json";

					HttpResponseMessage response = client.GetAsync(url).Result;
					if (response.IsSuccessStatusCode) {
						string json = response.Content.ReadAsStringAsync().Result;
						using (JsonDocument doc = JsonDocument.Parse(json)) {
							JsonElement root = doc.RootElement;
							JsonElement items = root.GetProperty("items")[0];
							JsonElement items2 = items.GetProperty("items");

							JsonElement latestPackage = items2[items2.GetArrayLength() - 1];
							string versionString = latestPackage.GetProperty("catalogEntry").GetProperty("version").GetString();

							return new Version(versionString);
						}
					}
				}
			} catch (Exception ex) {
				// Log error but don't throw - we want the process to continue
				// Just return null to indicate we couldn't get the version
			}

			return null;
		}

		public void OnAppEnd(AppEventContext context){
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