using System;
using System.Net;
using System.Net.Http;
using Common.Logging;
using MrktApolloApp.CustomException;
using Newtonsoft.Json;

namespace MrktApolloApp.RestClient
{
	internal class ResponseWrapper<T> where T : class, new()
	{

		#region Fields: Private

		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings {
			DateFormatHandling = DateFormatHandling.IsoDateFormat,
			NullValueHandling = NullValueHandling.Ignore
		};

		#endregion

		#region Constructors: Public 

		public ResponseWrapper(HttpResponseMessage responseMessage){
			HttpStatusCode = responseMessage.StatusCode;

			if (HttpStatusCode == (HttpStatusCode)699) {
				ErrorMessage = Messages.ApiKeyMissing;
				return;
			}
			
			if (HttpStatusCode == HttpStatusCode.Unauthorized) {
				ErrorMessage = Messages.Unauthorized;
				return;
			}
			
			if ((int)HttpStatusCode == 422) {
				ErrorMessage = Messages.InsufficientCredits;
				return;
			}
			
			if (HttpStatusCode == HttpStatusCode.Forbidden) {
				ErrorMessage = Messages.Forbidden;
				return;
			}
			
			if (HttpStatusCode != HttpStatusCode.OK) {
				ErrorMessage = Messages.Unknown;
				return;
			}
			string json = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			try {
				Data = JsonConvert.DeserializeObject<T>(json, _serializerSettings);
			} catch (Exception e) {
				Data = null;
				ErrorMessage = Messages.Unknown;
			}
		}

		#endregion

		#region Properties: Public

		public T Data { get; private set; }

		public string ErrorMessage { get; private set; }

		public HttpStatusCode HttpStatusCode { get; }

		#endregion

	}
}