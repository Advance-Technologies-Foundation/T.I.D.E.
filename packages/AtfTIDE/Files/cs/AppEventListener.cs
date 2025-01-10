using Terrasoft.Core;
using Terrasoft.Web.Common;

namespace AtfTIDE.ClioInstaller
{
	public class AppEventListener : IAppEventListener {

		public void OnAppStart(AppEventContext context) {
			AppConnection appConnection = context.Application["AppConnection"] as AppConnection;
			UserConnection userConnection = appConnection?.SystemUserConnection;
			userConnection.ProcessEngine.ProcessExecutor.Execute("AtfProcess_TryInstallClio");
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