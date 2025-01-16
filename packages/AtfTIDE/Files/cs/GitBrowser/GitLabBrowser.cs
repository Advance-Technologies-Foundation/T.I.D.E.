using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AtfTIDE.cs.GitBrowser
{
	public class Repository {
		public string Name { get; set; }

		public string Url { get; set; }
	}

	public class GitLabBrowser
	{
		private string gitlabUrl;

		public GitLabBrowser(string gitlabUrl) {
			this.gitlabUrl = gitlabUrl;
		}

		public async Task<List<Repository>> GetAllRepositoriesAsync(string gitlabToken) {
			var repositories = new List<Repository>();
			using (var client = new HttpClient()) {
				client.DefaultRequestHeaders.Add("Private-Token", gitlabToken);
				var response = await client.GetAsync($"{gitlabUrl}/api/v4/projects?per_page=100");
				if (response.IsSuccessStatusCode) {
					var content = await response.Content.ReadAsStringAsync();
					var json = JArray.Parse(content);
					foreach (var repo in json) {
						repositories.Add(new Repository {
							Name = repo["name"].ToString(),
							Url = repo["web_url"].ToString()
						});
					}
				} else {
					throw new Exception("Failed to fetch repositories");
				}
			}
			return repositories;
		}

		public List<string> GetAllRepositoriesNames(string gitlabToken) {
			var repo =  GetAllRepositoriesAsync(gitlabToken).Result;
			return repo.Select(r => r.Name).ToList();
		}

		internal List<Repository> FindClioRepositoriesOnServer(string gitlabToken) {
			var repositories = GetAllRepositoriesAsync(gitlabToken).Result;
			List<Repository> clioRepositories = new List<Repository>();
			foreach(var repo in repositories) {
				if (IsClioRepositories(repo,gitlabToken)) {
					clioRepositories.Add(repo);
				}
			}
			return clioRepositories;
		}

		private bool IsClioRepositories(Repository repo, string gitlabToken) {
			using (var client = new HttpClient()) {
				client.DefaultRequestHeaders.Add("Private-Token", gitlabToken);
				var response = client.GetAsync($"{repo.Url}/repository/tree?path=.clio").Result;
				return response.IsSuccessStatusCode;
			}
		}
	}

}
