using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
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
}
