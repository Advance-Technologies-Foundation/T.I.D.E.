using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http.Headers;
using AtfTIDE.ClioInstaller;
using AtfTIDE.GitBrowser;
using AtfTIDE.GitBrowser.GitLab;
using AtfTIDE.HttpClient;
using AtfTIDE.Logging;
using AtfTIDE.QueryExecutor;
using AtfTIDE.Services;
using Common.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AtfTIDE {
	public class TideApp {

		#region Fields: Private

		private static Lazy<TideApp> _instance
			= new Lazy<TideApp>(() => new TideApp());

		private Lazy<IInstaller> _installer;
		private readonly Lazy<ServiceProvider> _serviceProvider = new Lazy<ServiceProvider>(Init);

		#endregion

		#region Fields: Internal

		internal static IEnumerable<Func<IServiceCollection, IServiceCollection>> InjectedServices;

		#endregion

		#region Properties: Public

		public static TideApp Instance => _instance.Value;

		public IInstaller InstallerApp => GetInstallerApp();

		
		
		#endregion

		#region Methods: Private

		private static ServiceProvider Init(){
			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton(LogManager.GetLogger("AtfTide"));
			serviceCollection.AddSingleton<IInstaller, Installer>();
			serviceCollection.AddSingleton<IFileSystem, FileSystem>();
			serviceCollection.AddSingleton<ILiveLogger, LiveLogger>();
			serviceCollection.AddSingleton<IEsqFilterParser, EsqFilterParser>();
			serviceCollection.AddSingleton<IEsqColumnParser, EsqColumnParser>();
			serviceCollection.AddSingleton<IWebSocket, WebSocket>();
			serviceCollection.AddTransient<IUiCommandService, UiCommandService>();
			
			serviceCollection.AddTransient<IGitlabProvider, GitlabProvider>();
			serviceCollection.AddHttpClient("GitLab", client => {;
				client.Timeout = TimeSpan.FromSeconds(60);
				ProductInfoHeaderValue userAgentHeader =  new ProductInfoHeaderValue("Creatio_Tide", "1.0");
				client.DefaultRequestHeaders.UserAgent.Add(userAgentHeader);
			});
			
			
			serviceCollection.AddGitlabClient();
			serviceCollection.AddNugetClient();
			serviceCollection.AddGithubClient();
			InjectedServices?.ToList().ForEach(service => {
				service(serviceCollection);
			});
			return serviceCollection.BuildServiceProvider();
		}

		private IInstaller GetInstallerApp(){
			if (_installer?.Value == null) {
				IInstaller installer = _serviceProvider.Value.GetService<IInstaller>();
				_installer = new Lazy<IInstaller>(() => installer);
			}
			return _installer.Value;
		}

		#endregion

		#region Methods: Internal

		internal static TideApp Reset(){
			_instance = null;
			_instance = new Lazy<TideApp>(() => new TideApp());
			return _instance.Value;
		}

		#endregion

		#region Methods: Public

		public static TideApp Create() => Instance;

		public T GetKeyedService<T>(object serviceKey) => _serviceProvider.Value.GetKeyedService<T>(serviceKey);

		public T GetRequiredKeyedService<T>(object serviceKey) =>
			_serviceProvider.Value.GetRequiredKeyedService<T>(serviceKey);

		public T GetRequiredService<T>() => _serviceProvider.Value.GetRequiredService<T>();

		public T GetService<T>() => _serviceProvider.Value.GetService<T>();
		
		#endregion

	}
}
