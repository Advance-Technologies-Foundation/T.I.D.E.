using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AtfTIDE.ClioInstaller;
using AtfTIDE.ClioInstaller.Dto;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AtfTIDE.Tests.ClioInstaller {
	
	
	[TestFixture]
	public class NugetClientTests {

		private INugetClient _sut;
		private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();
		private HttpClient _mockClient;
		private MockHandler _mockHandler;
		
		[SetUp]
		public void SetUp(){
			_mockHandler = new MockHandler();
			//_mockClient = new HttpClient(_mockHandler);
			_mockClient = new HttpClient();
			_mockClient.BaseAddress = new Uri(TideConsts.NugetHttpBaseAddress);
			_factory.CreateClient(TideConsts.NugetHttpClientName)
					.Returns(_mockClient);
			_sut = new NugetClient(_factory);
		}

		[TearDown]
		public void TearDown(){
			_factory.ClearReceivedCalls();
		}
		
		[Test]
		public async Task GetModel(){
			ErrorOr<SearchResponse> modelOrError = await _sut.SearchAsync();
			modelOrError.IsError.Should().BeFalse();
			SearchResponse model = modelOrError.Value;
			model.TotalHits.Should().Be(1);
		}
		
		[Test]
		public async Task DownloadClioAsync(){
			ErrorOr<Success> modelOrError = await _sut.DownloadClioAsync("8.0.1.14");
		}
	}
	
	public class MockHandler : DelegatingHandler {
		
		public HttpResponseMessage Response { get; private set; }
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken){
			Response = await Task.FromResult(new HttpResponseMessage());
			Response.RequestMessage = request;
			string content = System.IO.File.ReadAllText("ClioInstaller/ResponseJson/SearchResponse.json"); 
			return new HttpResponseMessage() {
				Content = new StringContent(content),
				StatusCode = System.Net.HttpStatusCode.OK
			};
		}
	}
	
	
}