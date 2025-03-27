using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AtfTIDE.ClioInstaller;
using AtfTIDE.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AtfTIDE.Tests.HttpClient {
	
	[TestFixture]
	public class GitlabHttpClientTestFixture {

		private readonly List<Func<IServiceCollection,IServiceCollection>> _injectedServices = 
			new List<Func<IServiceCollection,IServiceCollection>>();
		
		[SetUp]
		public void Setup(){
			TideApp.Reset();
			TideApp.InjectedServices = _injectedServices;
		}
		
		
		[Test]
		public async Task TestOneAsync(){
			//Arrange
			MockDelegatingHandler handler = new MockDelegatingHandler();
			
			HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, "https://www.google.com");
			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent("Hello World");
			
			handler.AddMockResponse(msg, response);
			
			InjectorDelegatingHandler injectorDelegatingHandler = new InjectorDelegatingHandler(handler);
			_injectedServices.Add(injectorDelegatingHandler.AddTestHandler);
			

			//Act
			IGitLabHttpClient client = TideApp.Instance.GetRequiredService<IGitLabHttpClient>();

			//Assert
			var result = await client.GetGoogle();
			
		}

	}
}
