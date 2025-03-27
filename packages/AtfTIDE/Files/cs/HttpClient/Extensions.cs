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
