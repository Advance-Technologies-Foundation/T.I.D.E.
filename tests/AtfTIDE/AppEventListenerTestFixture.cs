using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtfTIDE.ClioInstaller;
using AtfTIDE.Tests.HttpClient;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace AtfTIDE.Tests {
	
	[MockSettings(RequireMock.All)]
	[TestFixture]
	public class AppEventListenerTestFixture : BaseMarketplaceTestFixture{

		private readonly List<Func<IServiceCollection,IServiceCollection>> _injectedServices = 
			new List<Func<IServiceCollection,IServiceCollection>>();
		private AppEventListener _sut;
		
		protected override void SetUp(){
			base.SetUp();
			TideApp.Reset();
			TideApp.InjectedServices = _injectedServices;
			
			MockEntitySchemaWithColumns("SysPackage", new Dictionary<string, DataValueType>() {
				{"Name", DataValueType.Text},
				{"Version", DataValueType.Text}
			});
			
			SetUpTestData("SysPackage", new Dictionary<string, object>() {
				{"Name", "AtfTide"},
				{"Version", "5.0.0"}
			});
			ClassFactory.RebindWithFactoryMethod(()=> (UserConnection)UserConnection);
			
			
			_sut = new AppEventListener();
		}
		
		[TestCase("4.0.0")]
		public void AppEventListener_Should_Find_Version(string mockNugetVersion){ 
			IInstaller installerMock = Substitute.For<IInstaller>();
			var success = Substitute.For<IErrorOr<Success>>();
			installerMock.InstallClio().Returns(success);
			
			InjectorInstaller injectorInstaller = new InjectorInstaller(installerMock);
			_injectedServices.Add(injectorInstaller.AddMockInstaller);
			
			INugetClient nugetClientMock = Substitute.For<INugetClient>();
			
			nugetClientMock.GetMaxVersionAsync("AtfTide")
							.Returns(Task.FromResult(mockNugetVersion.ToErrorOr()));
			
			InjectorNugetClient injectorNugetClient = new InjectorNugetClient(nugetClientMock);
			_injectedServices.Add(injectorNugetClient.AddMockNugetClient);
			
			_sut.OnAppStart(null);
			
			// Verify that the installer service is never called when Clio is installed
			installerMock.DidNotReceive().InstallClio();
		}
		
		
		private ErrorOr<Success> Success() => Result.Success;

	}
	
	public class A {

		public static ErrorOr<Success> B(){
			return Result.Success;
		}

	}
	
}
