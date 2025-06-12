using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AtfTIDE.HttpClient.GithubDto;

namespace AtfTIDE.HttpClient {
	
	
	public interface IGitHubClient {

		/// <summary>
		/// Lists repositories for the specified organization.
		/// </summary>
		/// <param name="organizationName">The name of the organization.</param>
		/// <returns></returns>
		/// <remarks>
		/// <see href="https://docs.github.com/en/rest/repos/repos#list-organization-repositories">See Github Docs</see>
		/// </remarks>
		Task<List<Repository>> ListOrganizationRepositories(string organizationName);

		
		/// <summary>
		/// Creates a new repository in the specified organization.
		/// </summary>
		/// <param name="organizationName">The name of the organization where the repository will be created.</param>
		/// <param name="repositoryName">The name of the new repository to create.</param>
		/// <returns>A <see cref="Task{Repository}"/> representing the asynchronous operation that returns the created repository.</returns>
		/// <remarks>
		/// <para>This method creates a public repository with a default description "this is a repository created by AtfTIDE".</para>
		/// <para>The repository is created with the following default settings:</para>
		/// <list type="bullet">
		/// <item><description>Public visibility (private = false)</description></item>
		/// <item><description>Default description provided by AtfTIDE</description></item>
		/// </list>
		/// <para><see href="https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#create-an-organization-repository">See GitHub API Documentation</see></para>
		/// </remarks>
		/// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
		/// <exception cref="JsonException">Thrown when JSON serialization/deserialization fails.</exception>
		Task<Repository> CreateOrganizationRepository(string organizationName, string repositoryName);
		
		
		/// <summary>
		/// Lists repositories for the authenticated user.
		/// </summary>
		/// <remarks>
		/// <see href="https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#list-repositories-for-the-authenticated-user">See Github Docs</see>
		/// </remarks>
		Task<object> ListUserRepositories();
		
		/// <summary>
		/// Creates a new repository for the authenticated user.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		/// <remarks>
		/// <see href="https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#create-a-repository-for-the-authenticated-user">See Github Docs</see>
		/// </remarks>
		Task<object> CreateUserRepository(string repositoryName);
	}
	
	public class GitHubClient : IGitHubClient{
		private readonly System.Net.Http.HttpClient _httpClient;
		public GitHubClient(System.Net.Http.HttpClient httpclient){
			_httpClient = httpclient;
		}
		public async Task<List<Repository>> ListOrganizationRepositories(string organizationName) {
			HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, $"/orgs/{organizationName}/repos");
			HttpResponseMessage result = await _httpClient.SendAsync(msg);
			if (!result.IsSuccessStatusCode) {
				throw new HttpRequestException($"Failed to list repositories for organization '{organizationName}': {result.ReasonPhrase}");
			}
			if (result.Content == null) {
				throw new HttpRequestException($"No content returned for organization '{organizationName}'");
			}
			using(Stream str = await result.Content.ReadAsStreamAsync()) {
				JsonSerializerOptions options = new JsonSerializerOptions {
					PropertyNameCaseInsensitive = true,
					Converters = {
						new Converters.JsonStringBoolConverter()
					}
				};
				List<Repository> repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(str, options);
				return repositories ?? new List<Repository>();
			}
		}
		
		
		public async Task<Repository> CreateOrganizationRepository(string organizationName, string repositoryName) {
			HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, $"/orgs/{organizationName}/repos");
			msg.Content = new StringContent(JsonSerializer.Serialize(new {
				name = repositoryName,
				description = "this is a repository created by AtfTIDE",
				@private = false,
			}), System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage result = await _httpClient.SendAsync(msg);
			
			using(Stream str = await result.Content.ReadAsStreamAsync()) {
				JsonSerializerOptions options = new JsonSerializerOptions {
					PropertyNameCaseInsensitive = true,
					Converters = {
						new Converters.JsonStringBoolConverter()
					}
				};
				Repository repository = await JsonSerializer.DeserializeAsync<Repository>(str, options);
				return repository;
			}
		}

		public Task<object> ListUserRepositories() {
			//TODO: Create return DTO, and implement this method
			throw new System.NotImplementedException();
		}

		public Task<object> CreateUserRepository(string repositoryName) {
			//TODO: Create return DTO, and implement this method
			throw new System.NotImplementedException();
		}

	}
}
