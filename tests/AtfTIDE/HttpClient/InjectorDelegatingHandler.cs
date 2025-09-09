using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
using AtfTIDE.ClioInstaller;
using Microsoft.Extensions.DependencyInjection;

namespace AtfTIDE.Tests.HttpClient {
	public class InjectorDelegatingHandler {

		private readonly DelegatingHandler _handler;

		public InjectorDelegatingHandler(DelegatingHandler handler){
			_handler = handler;
		}
		public IServiceCollection AddTestHandler(IServiceCollection services){
			services.AddKeyedTransient<DelegatingHandler>("testHandler", (sp,_)=>_handler);
			return services;
		}
	}
	
	public class InjectorFileSystem {

		private readonly IFileSystem _handler;

		public InjectorFileSystem(IFileSystem handler){
			_handler = handler;
		}
		public IServiceCollection AddMockFileSystem(IServiceCollection services){
			services.AddSingleton(_handler);
			return services;
		}
	}
	
	public class InjectorInstaller {

		private readonly IInstaller _installer;

		public InjectorInstaller(IInstaller installer){
			_installer = installer;
		}
		public IServiceCollection AddMockInstaller(IServiceCollection services){
			services.AddSingleton(_installer);
			return services;
		}
	}
	
	public class InjectorNugetClient {

		private readonly INugetClient _nugetClientMock;

		public InjectorNugetClient(INugetClient nugetClientMock){
			_nugetClientMock = nugetClientMock;
		}
		public IServiceCollection AddMockNugetClient(IServiceCollection services){
			services.AddSingleton(_nugetClientMock);
			return services;
		}
	}


	public class InjectorWebSocket{
		private readonly IWebSocket _webSocketMock;
		public InjectorWebSocket(IWebSocket webSocketMock){
			_webSocketMock = webSocketMock;
		}
		public IServiceCollection AddMockWebSocket(IServiceCollection services){
			services.AddSingleton(_webSocketMock);
			return services;
		}
	}
	
	
}
