using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AtfTIDE.ClioInstaller;
using AtfTIDE.ClioInstaller.Dto;
using AtfTIDE.Tests.HttpClient;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AtfTIDE.Tests.ClioInstaller {
	[TestFixture(Category = "NugetClient")]
	public class NugetClientTestsFixture{

		#region Setup/Teardown

		[SetUp]
		public void Setup(){
			TideApp.Reset();
			TideApp.InjectedServices = _injectedServices;
			Uri.TryCreate(TideConsts.NugetHttpBaseAddress, UriKind.Absolute, out _baseAddress);
			_handler = new MockDelegatingHandler();
			_mockFileSystem = InitMockFileSystem();
		}

		[TearDown]
		public void TearDown(){
			_mockFileSystem = null;
			_handler.Dispose();
			_handler = null;
			TideApp.Reset();
		}
		
		#endregion

		#region Fields: Private

		private readonly Dictionary<string, IDirectoryInfo> _defaultDirectories
			= new Dictionary<string, IDirectoryInfo>();

		private readonly List<Func<IServiceCollection, IServiceCollection>> _injectedServices =
			new List<Func<IServiceCollection, IServiceCollection>>();

		private Uri _baseAddress;
		private MockDelegatingHandler _handler;
		private MockFileSystem _mockFileSystem;

		#endregion

		#region Properties: Private

		private Action<HttpRequestMessage, HttpResponseMessage> MockResponse =>
			(request, response) => {
				_handler.AddMockResponse(request, response);
				InjectorDelegatingHandler injectorDelegatingHandler = new InjectorDelegatingHandler(_handler);
				_injectedServices.Add(injectorDelegatingHandler.AddTestHandler);
			};

		private Action<Dictionary<HttpRequestMessage, HttpResponseMessage>> MockResponses =>
			items => {
				foreach (KeyValuePair<HttpRequestMessage, HttpResponseMessage> item in items) {
					_handler.AddMockResponse(item.Key, item.Value);
				}
				InjectorDelegatingHandler injectorDelegatingHandler = new InjectorDelegatingHandler(_handler);
				_injectedServices.Add(injectorDelegatingHandler.AddTestHandler);
			};

		#endregion

		#region Methods: Private

		private MockFileSystem InitMockFileSystem(){
			_defaultDirectories.Clear();
			MockFileSystem mockFileSystem = new MockFileSystem();
			const string driveName = "X";
			MockDriveData drive = new MockDriveData {
				DriveType = DriveType.Fixed,
				VolumeLabel = driveName
			};
			mockFileSystem.AddDrive(drive.VolumeLabel, drive);
			string creatioDir = Path.Combine($"{driveName}:\\", "inetpub", "wwwroot", "creatio");
			mockFileSystem.AddDirectory(creatioDir);
			_defaultDirectories.Add(nameof(creatioDir), mockFileSystem.DirectoryInfo.New(creatioDir));
			
			string webappDir = Path.Join(creatioDir, "Terrasoft.WebApp");
			mockFileSystem.AddDirectory(webappDir);
			_defaultDirectories.Add(nameof(webappDir), mockFileSystem.DirectoryInfo.New(webappDir));

			string confDir = Path.Join(webappDir, "conf");
			mockFileSystem.AddDirectory(confDir);
			_defaultDirectories.Add(nameof(confDir), mockFileSystem.DirectoryInfo.New(confDir));

			InjectorFileSystem injectorFileSystem = new InjectorFileSystem(mockFileSystem);
			_injectedServices.Add(injectorFileSystem.AddMockFileSystem);
			return mockFileSystem;
		}

		#endregion

		[Test]
		public async Task DownloadClioAsync_Downloads_LatestVersion(){
			//Arrange
			const string requestPartialUrl = "/v3-flatcontainer/clio/index.json";
			Uri requestUri = new Uri(_baseAddress, requestPartialUrl);
			HttpRequestMessage searchRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
			string content = await File.ReadAllTextAsync("ClioInstaller/ResponseJson/clioVersions.json");
			HttpResponseMessage searchResponse = new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(content)
			};

			const string downloadRequestUrl = "v3-flatcontainer/clio/8.0.1.23/clio.8.0.1.23.nupkg";
			Uri downloadRequestUri = new Uri(_baseAddress, downloadRequestUrl);
			HttpRequestMessage downloadRequest = new HttpRequestMessage(HttpMethod.Get, downloadRequestUri);

			byte[] bytes = await File.ReadAllBytesAsync("ClioInstaller/ResponseJson/clio.8.0.1.23.nupkg");
			HttpResponseMessage downloadResponse = new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new ByteArrayContent(bytes)
			};
			MockResponses(new Dictionary<HttpRequestMessage, HttpResponseMessage> {
				{searchRequest, searchResponse},
				{downloadRequest, downloadResponse}
			});

			//Act
			INugetClient client = TideApp.Instance.GetRequiredService<INugetClient>();

			string clioDir = Path.Combine(_defaultDirectories["confDir"].FullName, "clio");
			await client.DownloadClioAsync(clioDir);

			//Assert
			IDirectoryInfo clioDireInfo = _mockFileSystem.DirectoryInfo.New(clioDir);
			clioDireInfo.Exists.Should().BeTrue();
		}

		[Test]
		public async Task DownloadClioAsync_Downloads_SpecificVersion(){
			//Arrange
			const string requestPartialUrl = "/v3-flatcontainer/clio/index.json";
			Uri requestUri = new Uri(_baseAddress, requestPartialUrl);
			HttpRequestMessage searchRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
			string content = await File.ReadAllTextAsync("ClioInstaller/ResponseJson/clioVersions.json");
			HttpResponseMessage searchResponse = new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(content)
			};

			const string downloadRequestUrl = "v3-flatcontainer/clio/8.0.1.23/clio.8.0.1.23.nupkg";
			Uri downloadRequestUri = new Uri(_baseAddress, downloadRequestUrl);
			HttpRequestMessage downloadRequest = new HttpRequestMessage(HttpMethod.Get, downloadRequestUri);

			byte[] bytes = await File.ReadAllBytesAsync("ClioInstaller/ResponseJson/clio.8.0.1.23.nupkg");
			HttpResponseMessage downloadResponse = new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new ByteArrayContent(bytes)
			};
			MockResponses(new Dictionary<HttpRequestMessage, HttpResponseMessage> {
				{searchRequest, searchResponse},
				{downloadRequest, downloadResponse}
			});

			//Act
			INugetClient client = TideApp.Instance.GetRequiredService<INugetClient>();
			string clioDir = Path.Combine(_defaultDirectories["confDir"].FullName, "clio");
			await client.DownloadClioAsync(clioDir, "8.0.1.23");

			//Assert
			IDirectoryInfo clioDireInfo = _mockFileSystem.DirectoryInfo.New(clioDir);
			clioDireInfo.Exists.Should().BeTrue();
		}

		[Test]
		public async Task SearchAsync_Returns_DeserializedModel(){
			//Arrange
			const string requestPartialUrl = "/query?q=clio&packageType=DotnetTool";
			Uri requestUri = new Uri(_baseAddress, requestPartialUrl);
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
			string content = await File.ReadAllTextAsync("ClioInstaller/ResponseJson/SearchResponse.json");
			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(content)
			};
			MockResponse(request, response);

			//Act
			INugetClient client = TideApp.Instance.GetRequiredService<INugetClient>();
			ErrorOr<SearchResponse> isErrorResult = await client.SearchAsync();

			//Assert
			isErrorResult.IsError.Should().BeFalse();
			SearchResponse result = isErrorResult.Value;
			result.Should().NotBeNull();
			result.Should().BeOfType<SearchResponse>();
			result.Data.Should().HaveCount(1);
			result.TotalHits.Should().Be(1);
		}

	}
}
