using System;
using System.Net.Http;
using AtfTIDE.ClioInstaller;
using Microsoft.Extensions.DependencyInjection;

namespace AtfTIDE.HttpClient {
	public static class Extensions {

		public static IServiceCollection AddGitlabClient(this IServiceCollection services){
			services.AddHttpClient<IGitLabHttpClient, GitLabHttpClient>(client => { })
					.ConfigurePrimaryHttpMessageHandler(provider =>
						provider.GetKeyedService<DelegatingHandler>("testHandler") as HttpMessageHandler ??
						new HttpClientHandler());
			return services;
		}

		public static IServiceCollection AddGithubClient(this IServiceCollection services){
			services.AddKeyedTransient<GithubTokenHandler>(TideConsts.GithubClientTokenHandlerName);
			services
				.AddHttpClient<IGitHubClient, GitHubClient>(TideConsts.GithubClientName,client => {
					client.BaseAddress = new Uri("https://api.github.com", UriKind.Absolute);
				})
				.ConfigurePrimaryHttpMessageHandler(provider => {
					HttpMessageHandler handler = provider.GetKeyedService<GithubTokenHandler>(TideConsts.GithubClientTokenHandlerName) as HttpMessageHandler ??
						new HttpClientHandler();
					return handler;
				});
			return services;
		}

		
		
		
		public static IServiceCollection AddNugetClient(this IServiceCollection services){
			services.AddHttpClient<INugetClient, NugetClient>(client => {
						client.BaseAddress = new Uri(TideConsts.NugetHttpBaseAddress);
						client.DefaultRequestHeaders.Add("User-Agent", "Creatio");
					})
					.ConfigurePrimaryHttpMessageHandler(provider =>
						provider.GetKeyedService<DelegatingHandler>("testHandler") as HttpMessageHandler ??
						new HttpClientHandler());
			return services;
		}

	}
}
