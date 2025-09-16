using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AtfTIDE.Logging;
using ErrorOr;
using Terrasoft.Common;

namespace AtfTIDE.GitBrowser.GitLab{

	public interface IGitlabProvider{
		IConfiguredProvider Configure(Uri gitlabUrl, string gitlabToken);
	}

	public interface IConfiguredProvider{
		Task GetAllRepositoriesAsync();
		Task<ErrorOr<IEnumerable<ProjectDto>>> GetAllRepositoriesByFilterAsync(string filter, int rowCount, int skipRowCount);

	}

	public class GitlabProvider: IGitlabProvider, IConfiguredProvider{
		private readonly ILiveLogger _logger;

		private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() {
			AllowTrailingCommas = true,
			MaxDepth = 10,
			PropertyNameCaseInsensitive = true
		};
		
		
		private readonly System.Net.Http.HttpClient _client;
		public GitlabProvider(IHttpClientFactory factory, ILiveLogger logger) {
			_logger = logger;
			_client = factory.CreateClient("GitLab");
		}

		public async Task GetAllRepositoriesAsync() {
			var a = "";
		}
		
		public async Task<ErrorOr<IEnumerable<ProjectDto>>> GetAllRepositoriesByFilterAsync(string filter, int rowCount, int skipRowCount) {

			string currentStep = "start";
			if (string.IsNullOrWhiteSpace(filter)) {
				return new List<ProjectDto>();
			}
			int page = (skipRowCount / rowCount + 1);
			//var q = System.Web.HttpUtility.ParseQueryString($"/api/v4/projects?pagination=yes&per_page={rowCount}&page={page}&topic=composable-application&search={filter}");
			try {
				currentStep = "building url";
				NameValueCollection queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
				queryParams["pagination"] = "yes";
				queryParams["per_page"] = rowCount.ToString();
				queryParams["page"] = page.ToString();
				queryParams["search"] = filter;
				UriBuilder uriBuilder = new UriBuilder(_client.BaseAddress) {
					Path = "/api/v4/projects",
					Query = queryParams.ToString()
				};
				
				currentStep = "Calling GitLab";
				HttpResponseMessage result = await _client.GetAsync(uriBuilder.Uri.PathAndQuery);
				if (result.StatusCode != HttpStatusCode.OK) {
					currentStep = "Deserializing Error response";
					string errorString = await result.Content.ReadAsStringAsync();
					string errorMessage = $"Failed to get projects from GitLab: {result.StatusCode} {errorString}";
					return Error.Failure("GetAllRepositoriesByFilterAsync", errorMessage);
				}
			
				currentStep = "Deserializing OK response";
				Stream stream = await result.Content.ReadAsStreamAsync();
				List<ProjectDto> resultList = await JsonSerializer.DeserializeAsync<List<ProjectDto>>(stream, JsonSerializerOptions);
				return resultList;
			}
			catch (Exception e) {
				string message = $"{GetType().Name} Failed to get projects from GitLab on {currentStep}: {e.Message}";
				return ErrorOr.Error.Failure("GetAllRepositoriesByFilterAsync", message);
			}
			
		}
		
		public IConfiguredProvider Configure(Uri gitlabUrl, string gitlabToken) {
			if (gitlabUrl == null) {
				_logger.LogError("GitLab URL is not configured, can not continue");
			}
			_client.BaseAddress = gitlabUrl;
			if (!string.IsNullOrWhiteSpace(gitlabToken)) {
				_client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", gitlabToken);
			}
			return this;

		}
	}

}
