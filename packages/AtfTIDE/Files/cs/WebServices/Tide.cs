using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.SessionState;
using Common.Logging;
using Terrasoft.Web.Common;
using Terrasoft.Web.Http.Abstractions;

namespace AtfTIDE.WebServices {
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	internal class Tide : BaseService, IReadOnlySessionState {

		/// <summary>
		/// This endpoint captures arguments required for clio to make callback.
		/// </summary>
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
					BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public string CaptureClioArgs(){
			
			HttpRequest request = HttpContextAccessor.GetInstance().Request;
			HttpCookieCollection cookies = request.Cookies;
			
			Dictionary<string, string> sysInfo = new Dictionary<string, string>();
			string systemUrl = WebUtilities.GetBaseApplicationUrl(HttpContextAccessor.GetInstance().Request);
			if(systemUrl.EndsWith("/0", StringComparison.InvariantCulture)) {
				sysInfo.Add("IsFramework", "true");
			}else {
				sysInfo.Add("IsFramework", "false");
			}
			sysInfo.Add("SystemUrl", systemUrl);
			var logger = LogManager.GetLogger("AtfTIDE");
			foreach (string cookieName in cookies.Keys) {
				logger.InfoFormat("{0}={1};", cookieName, cookies[cookieName].Value);
				sysInfo.Add(cookieName, cookies[cookieName].Value);
			}
			
			HelperFunctions.AddClioArgsForUser(UserConnection.CurrentUser.Id, sysInfo);
			return string.Empty;
		}

	}
}