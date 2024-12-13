using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MrktApolloApp.CustomException;
using Terrasoft.Core;

namespace MrktApolloApp.RestClient
{

	public class ApiKeyMessageHandler : DelegatingHandler
	{

		private readonly UserConnection _userConnection;
		private const string ApiKeySysSettingCode = "MrktApolloApiKey";
		
		public ApiKeyMessageHandler(UserConnection userConnection){
			_userConnection = userConnection;
		}

		/// <summary>
		/// Adds api_key to every request
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string apiKeyHeaderValue = Terrasoft.Core.Configuration.SysSettings.TryGetValue(_userConnection, ApiKeySysSettingCode, out object value) 
				? value?.ToString() ?? string.Empty 
				: string.Empty;
			
			HttpResponseMessage missingApiKeyResponse = new HttpResponseMessage((HttpStatusCode)699) {
				Content = new StringContent(Messages.ApiKeyMissing)
			};
			
			if(string.IsNullOrEmpty(apiKeyHeaderValue)) {
				return Task.FromResult(missingApiKeyResponse);
			}
			
			const string apiKeyHeaderName = "X-Api-Key";
			if(request.Headers.Contains(apiKeyHeaderName)) {
				request.Headers.Remove(apiKeyHeaderName);
			}
			request.Headers.Add(apiKeyHeaderName, apiKeyHeaderValue);
			
			const string cacheControlHeaderName = "Cache-Control";
			const string cacheControlHeaderValue = "no-cache";
			if(request.Headers.Contains(cacheControlHeaderName)) {
				request.Headers.Remove(cacheControlHeaderName);
			}
			request.Headers.Add(cacheControlHeaderName, cacheControlHeaderValue);
			
			if(request.Content!= null && request.Method != HttpMethod.Get) {
				request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			}
			return base.SendAsync(request, cancellationToken);
		}
	}
}