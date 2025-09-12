using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AtfTIDE.Logging;
using Newtonsoft.Json.Linq;

namespace AtfTIDE.GitBrowser
{
	public class Repository {
		public string Name { get; set; }
		public string UrlToClone { get; set; }
		public string WebUrl { get; set; }
		public string Id
		{
			get;
			internal set;
		}
	}

	public class GitLabBrowser
	{
		private readonly string _gitlabUrl;
		private readonly ILiveLogger _liveLogger;

		public int MaxRepositoryCount { get; set; } = int.MaxValue;

		public GitLabBrowser(string gitlabUrl) {
			_gitlabUrl = gitlabUrl;
			_liveLogger = TideApp.Instance.GetRequiredService<ILiveLogger>();
		}

		public async Task<List<Repository>> GetAllRepositoriesAsync(string gitlabToken) {
			ConcurrentBag<Repository> repositories = new ConcurrentBag<Repository>();
			
			using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()) {
				client.DefaultRequestHeaders.Add("Private-Token", gitlabToken);
				int page = 1;
				bool hasMorePages = true;
				List<Task> tasks = new List<Task>();
				while (hasMorePages && repositories.Count < MaxRepositoryCount) {
					_liveLogger.LogInfo($"Reading repositories... Page: {page}");
					HttpResponseMessage response = await client.GetAsync($"{_gitlabUrl}/api/v4/projects?per_page=100&page={page}");
					if (response.IsSuccessStatusCode) {
						string content = await response.Content.ReadAsStringAsync();
						JArray json = JArray.Parse(content);
						
						if (json.Count == 0) {
							hasMorePages = false;
						} 
						else {
							tasks.Add(Task.Run(() => {
								foreach (JToken repo in json) {
									repositories.Add(new Repository {
										Name = repo["name"].ToString(),
										UrlToClone = repo["http_url_to_repo"].ToString(),
										WebUrl = repo["web_url"].ToString(),
										Id = repo["id"].ToString()
									});
								}
							}));
							page++;
						}
					} else {
						throw new Exception("Failed to fetch repositories");
					}
				}
				await Task.WhenAll(tasks);
			}
			_liveLogger.LogInfo($"Finished reading {repositories.Count} repositories");
			return repositories.ToList();
		}


		public List<string> GetAllRepositoriesNames(string gitlabToken) {
			var repo =  GetAllRepositoriesAsync(gitlabToken).Result;
			return repo.Select(r => r.Name).ToList();
		}

		internal IEnumerable<Repository> FindClioRepositoriesOnServer(string gitlabToken) {
			var repositories = GetAllRepositoriesAsync(gitlabToken).Result;
			ConcurrentBag<Repository> clioRepositories = new ConcurrentBag<Repository>();
			Parallel.ForEach(repositories, new ParallelOptions() {
				MaxDegreeOfParallelism = Environment.ProcessorCount } , (repo) => {
					if (IsClioRepositories(repo,gitlabToken)) {
						clioRepositories.Add(repo);
					}
			});
			return clioRepositories;
		}

		public bool IsClioRepositories(Repository repo, string gitlabToken) {
			
			_liveLogger.LogInfo($"Checking repository {repo.Name} for being a Clio repository");
			
			using (var client = new System.Net.Http.HttpClient()) {
				client.DefaultRequestHeaders.Add("Private-Token", gitlabToken);
				if (string.IsNullOrEmpty(repo.Id)) {
					var response = client.GetAsync($"{_gitlabUrl}/api/v4/projects?search={repo.Name}").Result;
					if (response.IsSuccessStatusCode) {
						var content = response.Content.ReadAsStringAsync().Result;
						var json = JArray.Parse(content);
						if (json.Count > 0) {
							repo.Id = json[0]["id"].ToString();
						} else {
							return false;
						}
					} else {
						return false;
					}
				}
				var repoResponse = client.GetAsync($"{_gitlabUrl}/api/v4/projects/{repo.Id}/repository/tree?path=.clio").Result;
				if (repoResponse.IsSuccessStatusCode) {
					var repoContent = repoResponse.Content.ReadAsStringAsync().Result;
					var repoJson = JArray.Parse(repoContent);
					return repoJson.Count > 0;
				}
				return false;
			}
		}
	}

}
