using Terrasoft.Core;
using Terrasoft.Core.Configuration;
using Terrasoft.Web.Common;

namespace AtfTIDE.ClioInstaller {
	public class AppEventListener : IAppEventListener {

		public void OnAppStart(AppEventContext context){
			
			AppConnection appConnection = context.Application["AppConnection"] as AppConnection;
			UserConnection userConnection = appConnection?.SystemUserConnection; // System UserConnection
			if(userConnection == null){
				return;
			}
			string filePath = (string)SysSettings.GetValue(userConnection, TideConsts.SysSettingClioPath);
			if(string.IsNullOrEmpty(filePath)){
				TideApp.Create().InstallerApp.InstallClio();
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