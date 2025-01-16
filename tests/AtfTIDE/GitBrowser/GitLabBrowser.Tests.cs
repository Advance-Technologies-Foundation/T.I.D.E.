using AtfTIDE.cs.GitBrowser;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtfTIDE.Tests.GitBrowser
{
	[TestFixture]
	internal class GitLabBrowserTests
	{
		[Test]
		public async Task FindAllRepositoriesOnServer() {
			
			var gitlabBrowser = new GitLabBrowser(gitlabUrl);
			List<string> repositories = gitlabBrowser.GetAllRepositoriesNames(gitlabToken);
			Assert.Contains("labKirill", repositories);
		}

		[Test]
		public async Task FindClioWorkspacesOnServer() {

			var gitlabBrowser = new GitLabBrowser(gitlabUrl);
			List<Repository> repositories = gitlabBrowser.FindClioRepositoriesOnServer(gitlabToken);
			repositories.Should().Contain(r => r.Name == "labKirill");
		}
	}
}
