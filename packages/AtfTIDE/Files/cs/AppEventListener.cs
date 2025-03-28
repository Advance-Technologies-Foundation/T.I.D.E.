using System;
using System.Globalization;
using System.IO;
using System.Linq;
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

		public void OnAppStart(AppEventContext context) {
			var userConnection = ClassFactory.Get<UserConnection>(); //Who is this ?
			
			IErrorOr<Success> result = TideApp.Instance.InstallerApp.InstallClio();
			if(result.IsError) {
				
				
			}else {
				DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
				FileInfo[] clioFilePath = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
				SysSettings.SetValue(userConnection, "AtfClioFilePath",clioFilePath.First().FullName);
			}
			
			
			var maybeMaxTideVersion = TideApp.Instance.GetRequiredService<INugetClient>()
					.GetMaxVersionAsync("AtfTide")
					.GetAwaiter().GetResult();
				
			if(maybeMaxTideVersion.IsError) {
				LogManager.GetLogger("AtfTide").ErrorFormat(CultureInfo.InvariantCulture, $"{maybeMaxTideVersion.FirstError.Code} - {maybeMaxTideVersion.FirstError.Description}");
				return; //Add Logging
			}
			string nugetMaxTideVersion = maybeMaxTideVersion.Value;
			
			
			EntitySchemaQuery esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "SysPackage");
			esq.AddAllSchemaColumns();
			
			IEntitySchemaQueryFilterItem nameFilter =
				esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Name", "AtfTide");
			esq.Filters.Add(nameFilter);
			
			
			var collection = esq.GetEntityCollection(userConnection);
			if(collection.Count == 1) {
				var version = collection[0].GetTypedColumnValue<string>("Version");
				
				var installedV = Version.Parse(version);
				var nugetV = Version.Parse(nugetMaxTideVersion);
				bool updateAvailable = nugetV.CompareTo(installedV) == 1;
				
				LogManager.GetLogger("AtfTide")
						.InfoFormat(CultureInfo.InvariantCulture,  $"Updating SysSetting AtfTideUpdateAvailable to: {updateAvailable}, AtfTideVersion: {version}, NugetMaxTideVersion: {nugetMaxTideVersion}");
				SysSettings.SetDefValue(userConnection, "AtfTideUpdateAvailable", updateAvailable);
			}
			
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