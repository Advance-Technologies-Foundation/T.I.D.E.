using System.Collections.Generic;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	
	public interface ITextTransformer {

		string Transform(string text);

	}
	
	[DefaultBinding(typeof(ITextTransformer), Name = "UrlAppender")]
	public class UrlAppender : ITextTransformer {

		public string Transform(string text) {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Dictionary<string, string> webSettings = HelperFunctions.ClioArguments[userConnection.CurrentUser.Id];
			string url = webSettings["SystemUrl"]; //http:localhost:8080/0 ??http:localhost:8080 
			bool isFramework = url.EndsWith("/0");
			url = url.TrimEnd("/0".ToCharArray());
			return $"{text} -u {url} --IsNetCore {!isFramework}";
		}
	}
}