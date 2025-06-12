using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AtfTIDE.HttpClient {
	public class GithubTokenHandler : DelegatingHandler {

		private const string AcceptHeaderValue = "application/vnd.github.v3+json";
		public GithubTokenHandler(): base(new HttpClientHandler()) {
			// Default constructor initializes the base class with a new HttpClientHandler
			// This allows us to use this handler in an HttpClient pipeline
		}
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			
			// see https://docs.github.com/en/rest/using-the-rest-api/getting-started-with-the-rest-api?apiVersion=2022-11-28#accept
			if(request.Headers.Accept == null) {
				request.Headers.Add("Accept", AcceptHeaderValue);
			}
			
			if(request.Headers.Accept != null && request.Headers.Accept.Count == 0 ) {
				MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue(AcceptHeaderValue);
				request.Headers.Accept.Add(acceptHeader);
			}
			
			//seehttps://docs.github.com/en/rest/using-the-rest-api/getting-started-with-the-rest-api?apiVersion=2022-11-28#user-agent
			request.Headers.UserAgent.Clear();
			request.Headers.UserAgent.Add(new ProductInfoHeaderValue("AtfTide-Creatio", "1.0"));
			
			// Kirill's header
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "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");
			
			return base.SendAsync(request, cancellationToken);
		}

	}
}
