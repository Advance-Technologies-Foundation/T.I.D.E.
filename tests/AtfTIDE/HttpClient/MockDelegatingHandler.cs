using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace AtfTIDE.Tests.HttpClient {
	public class MockDelegatingHandler : DelegatingHandler {
		public MockDelegatingHandler(): base(new HttpClientHandler()) { }
		
		private readonly Dictionary<HttpRequestMessage, HttpResponseMessage> _mockResponses = 
			new Dictionary<HttpRequestMessage, HttpResponseMessage>();
		public void AddMockResponse(HttpRequestMessage request, HttpResponseMessage response) {
			_mockResponses.Add(request, response);
		}
		
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			HttpRequestMessage key = _mockResponses.Keys.
													FirstOrDefault(k=> k.RequestUri == request.RequestUri && k.Method == request.Method);
			return key != null 
				? Task.FromResult(_mockResponses[key]) 
				: Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
		}
	}
}
