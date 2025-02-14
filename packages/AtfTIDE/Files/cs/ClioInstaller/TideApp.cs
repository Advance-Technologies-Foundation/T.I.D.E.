using System;
using AtfTIDE.QueryExecutor;
using Microsoft.Extensions.DependencyInjection;
using Terrasoft.Core.Factories;

namespace AtfTIDE.ClioInstaller {
	
	
	public class TideApp{

		public IInstaller InstallerApp => GetInstallerApp();
		private Lazy<IInstaller> _installer;
		
		private readonly IServiceProvider _serviceProvider;

		private TideApp(){
			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton<IInstaller, Installer>();
			serviceCollection.AddHttpClient(TideConsts.NugetHttpClientName, client => {
				client.BaseAddress = new Uri(TideConsts.NugetHttpBaseAddress);
				client.DefaultRequestHeaders.Add("User-Agent", "Creatio");
			});
			serviceCollection.AddSingleton<IEsqFilterParser, EsqFilterParser>();
			serviceCollection.AddSingleton<IEsqColumnParser, EsqColumnParser>();
			serviceCollection.AddTransient<INugetClient, NugetClient>();
			_serviceProvider = serviceCollection.BuildServiceProvider();
		}
		private IInstaller GetInstallerApp(){
			if(_installer?.Value == null){
				IInstaller installer = _serviceProvider.GetService<IInstaller>();
				_installer = new Lazy<IInstaller>(() => installer);
			}
			return _installer.Value;
		}
		
		public T GetService<T>() => _serviceProvider.GetService<T>();
		
		public static TideApp Create() => new TideApp();
	}
}