using System;
using System.Net.Http;
using System.Threading.Tasks;
using AtfTIDE.Logging;

namespace AtfTIDE.GitBrowser.GitLab{

	public interface IGitlabProvider{
		IConfiguredProvider Configure(Uri gitlabUrl, string gitlabToken);
	}

	public interface IConfiguredProvider{
		Task GetAllRepositoriesAsync();
		
	}

	public class GitlabProvider: IGitlabProvider, IConfiguredProvider{
		private readonly ILiveLogger _logger;

		private readonly System.Net.Http.HttpClient _client;
		public GitlabProvider(IHttpClientFactory factory, ILiveLogger logger) {
			_logger = logger;
			_client = factory.CreateClient("GitLab");
		}

		public async Task GetAllRepositoriesAsync() {
			var a = "";
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
