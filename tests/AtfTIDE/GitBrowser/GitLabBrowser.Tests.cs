using AtfTIDE.cs.GitBrowser;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AtfTIDE.Tests.GitBrowser
{
	[TestFixture]
	internal class GitLabBrowserTests
	{
		private static string FindConfigFilePath(string fileName) {
			string directory = AppDomain.CurrentDomain.BaseDirectory;
			while (directory != null) {
				string filePath = Path.Combine(directory, fileName);
				if (File.Exists(filePath)) {
					return filePath;
				}
				directory = Directory.GetParent(directory)?.FullName;
			}
			throw new FileNotFoundException($"Configuration file '{fileName}' not found.");
		}

		private static string ConfigFilePath = FindConfigFilePath("Tide.settings");

		private static dynamic LoadConfig() {
			using (StreamReader r = new StreamReader(ConfigFilePath)) {
				string json = r.ReadToEnd();
				return JsonConvert.DeserializeObject(json);
			}
		}

		private static readonly dynamic Config = LoadConfig();
		private static readonly string GitlabUrl = Config.GitlabUrl;
		private static readonly string GitlabToken = Config.GitlabToken;
		private static readonly string StudioUrl = Config.StudioUrl;
		private static readonly string StudioFreeThemeUrl = Config.StudioFreeThemeUrl;

		[Test]
		public async Task FindAllRepositoriesOnServer() {
			var gitlabBrowser = new GitLabBrowser(GitlabUrl);
			List<string> repositories = gitlabBrowser.GetAllRepositoriesNames(GitlabToken);
			Assert.Contains("labKirill", repositories);
		}

		[Test]
		public async Task FindClioWorkspacesOnServer() {
			var gitlabBrowser = new GitLabBrowser(GitlabUrl);
			gitlabBrowser.MaxRepositoryCount = 100;
			var repositories = gitlabBrowser.FindClioRepositoriesOnServer(GitlabToken);
			repositories.Should().Contain(r => r.Name == "labKirill"
				&& r.UrlToClone.Contains("k.krylov/labkirill.git")
				&& r.WebUrl.Contains("k.krylov/labkirill"));
		}

		[Test]
		public async Task FindClioWorkspacesOnServer_ShouldBeFalse() {
			var gitlabBrowser = new GitLabBrowser(GitlabUrl);
			gitlabBrowser.MaxRepositoryCount = 100;
			Repository repository = new Repository();
			repository.WebUrl = StudioFreeThemeUrl;
			repository.Name = "studio-free-theme";
			Assert.IsFalse(gitlabBrowser.IsClioRepositories(repository, GitlabToken));
		}

		[Test]
		public async Task FindClioWorkspacesOnServerByName() {
			var gitlabBrowser = new GitLabBrowser(GitlabUrl);
			gitlabBrowser.MaxRepositoryCount = 100;
			Repository repository = new Repository();
			repository.WebUrl = StudioUrl;
			repository.Name = "labkirill";
			Assert.IsTrue(gitlabBrowser.IsClioRepositories(repository, GitlabToken));
		}

		[Test]
		public async Task FindClioWorkspacesOnServerById() {
			var gitlabBrowser = new GitLabBrowser(GitlabUrl);
			gitlabBrowser.MaxRepositoryCount = 100;
			Repository repository = new Repository();
			repository.WebUrl = StudioUrl;
			repository.Id = "1342";
			Assert.IsTrue(gitlabBrowser.IsClioRepositories(repository, GitlabToken));
		}

		[Test]
		public async Task FindClioWorkspacesOnServerByIdShouldBeFalse() {
			var gitlabBrowser = new GitLabBrowser(GitlabUrl);
			gitlabBrowser.MaxRepositoryCount = 100;
			Repository repository = new Repository();
			repository.WebUrl = StudioUrl;
			repository.Id = "239";
			Assert.IsFalse(gitlabBrowser.IsClioRepositories(repository, GitlabToken));
		}
	}
}
