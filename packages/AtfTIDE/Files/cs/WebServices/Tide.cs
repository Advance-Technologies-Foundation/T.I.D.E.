using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.SessionState;
using Terrasoft.Web.Common;
using Terrasoft.Web.Http.Abstractions;

namespace AtfTIDE.WebServices {
	
	/// <summary>
	/// Provides web service functionality for the AtfTIDE system with <c>ConsoleGit.exe</c> integration capabilities.
	/// This service operates in read-only mode with respect to session state.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item>This service class implements WCF service contract</item>
	/// <item>Requires ASP.NET compatibility mode</item>
	/// <item>Inherits from BaseService and implements IReadOnlySessionState for secure session handling</item>
	/// </list>
	/// </remarks>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class Tide : BaseService, IReadOnlySessionState {

		/// <summary>
		/// Captures and processes arguments required for <c>ConsoleGit.exe</c> functionality.
		/// </summary>
		/// <remarks>
		/// This endpoint is called from <strong><c>AtfTIDE_FormPage</c></strong> before start of any Business Process.
		/// It captures system and user information to be used in UserTasks of a process.
		/// This endpoint performs the following operations:
		/// <list type="table">
		/// <listheader>
		/// <term>Operation Steps</term>
		/// <description>Detailed process flow</description>
		/// </listheader>
		/// <item>
		/// <term>Request Retrieval</term>
		/// <description>Retrieves the current HTTP request and its cookies</description>
		/// </item>
		/// <item>
		/// <term>System Information</term>
		/// <description>Collects system information including the base application URL</description>
		/// </item>
		/// <item>
		/// <term>Framework Detection</term>
		/// <description>Determines if the system is running in framework mode</description>
		/// </item>
		/// <item>
		/// <term>Data Storage</term>
		/// <description>Stores the collected information for the current user</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <returns>
		/// Returns NoContent - 204.
		/// </returns>
		/// <example>
		/// This endpoint can be called via a GET request:
		/// <code>
		/// GET <c>rest/Tide/CaptureClioArgs</c> HTTP/1.1
		/// </code>
		/// </example>
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
					BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public void CaptureClioArgs(){
			HttpRequest request = HttpContextAccessor.GetInstance().Request;
			HttpCookieCollection cookies = request.Cookies;
			Dictionary<string, string> sysInfo = new Dictionary<string, string>();
			string systemUrl = WebUtilities.GetBaseApplicationUrl(HttpContextAccessor.GetInstance().Request);
			sysInfo.Add("IsFramework", systemUrl.EndsWith("/0", StringComparison.InvariantCulture) ? "true" : "false");
			sysInfo.Add("SystemUrl", systemUrl);
			foreach (string cookieName in cookies.Keys) {
				sysInfo.Add(cookieName, cookies[cookieName].Value);
			}
			HelperFunctions.AddClioArgsForUser(UserConnection.CurrentUser.Id, sysInfo);

#if NETSTANDARD2_0
			var response = HttpContextAccessor.GetInstance().Response;
			response.StatusCode = 204;
#else			
			WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
#endif			
			
		}

	}
}