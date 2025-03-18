using System;
using Terrasoft.Web.Common;

namespace AtfTIDE
{
	public class AppEventListener : IAppEventListener {

		public void OnAppStart(AppEventContext context) {
			TideApp.Instance.InstallerApp.InstallClio();
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