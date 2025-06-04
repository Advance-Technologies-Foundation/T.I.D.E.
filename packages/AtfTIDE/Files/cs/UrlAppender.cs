using System.Collections.Generic;
using Terrasoft.Core;
using Terrasoft.Core.Configuration;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	
	public interface ITextTransformer {

		string Transform(string text);
		
		/// <summary>
		/// Appends UserName and Password from SysSettings
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		/// <remarks>
		/// <list type="bullet">
		/// <item>Uses value from <c>AtfUserNameForClio</c> sysSetting or <b><c>Supervisor</c></b></item>
		/// <item>Uses value from <c>AtfPasswordForClio</c> sysSetting or <b><c>Supervisor</c></b></item>
		/// </list>
		/// </remarks>
		string TransformWithLP(string text);

	}
	
	[DefaultBinding(typeof(ITextTransformer), Name = "UrlAppender")]
	public class UrlAppender : ITextTransformer {

		public string Transform(string text) {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Dictionary<string, string> webSettings = HelperFunctions.ClioArguments[userConnection.CurrentUser.Id];
			string url = webSettings["SystemUrl"]; //http:localhost:8080/0 ??http:localhost:8080 
			bool isFramework = url.EndsWith("/0");
			if(isFramework) {
				url = url.Substring(0,url.Length - 2);
			}
			return $"{text} -u {url} --IsNetCore {!isFramework} --log creatio";
		}
		
		public string TransformWithLP(string text) {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Dictionary<string, string> webSettings = HelperFunctions.ClioArguments[userConnection.CurrentUser.Id];
			string url = webSettings["SystemUrl"]; //http:localhost:8080/0 ??http:localhost:8080 
			bool isFramework = url.EndsWith("/0");
			if(isFramework) {
				url = url.Substring(0,url.Length - 2);
			}
			return $"{text} -u {url} -l {GetCreatioUserName()} -p {GetCreatioPassword()} --IsNetCore {!isFramework} --log creatio";
		}
		public string GetCreatioUserName() {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			string userName = SysSettings
				.GetValue(userConnection, "AtfUserNameForClio", "Supervisor");
			return userName;
			
		}
		public string GetCreatioPassword() {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			string userName = SysSettings
				.GetValue(userConnection, "AtfPasswordForClio", "Supervisor");
			return userName;
		}
	}
	
	
	
}