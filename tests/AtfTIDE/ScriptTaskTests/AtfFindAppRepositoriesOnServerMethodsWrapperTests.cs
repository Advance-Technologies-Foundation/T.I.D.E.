using System;
using System.Collections.Generic;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Process;

namespace AtfTIDE.Tests.ScriptTaskTests {
	
	[TestFixture]
	[MockSettings(RequireMock.All)]
	public class AtfFindAppRepositoriesOnServerMethodsWrapperTests : BaseMarketplaceTestFixture {
		protected override void SetUp() {
			base.SetUp();
			
			
			Dictionary<string, DataValueType> columns = new Dictionary<string, DataValueType>() {
				{"AtfName",DataValueType.Text},
				{"AtfAccessToken",DataValueType.Text},
				{"AtfRepositoryUrl",DataValueType.Text},
				{"AtfUserName",DataValueType.Text},
			};
			MockEntitySchemaWithColumns("AtfGitServer", columns);
			SetUpTestData("AtfGitServer", new Dictionary<string, object> {
				{"AtfName", "Fake-repo"},
				{"AtfAccessToken", "FakeToken"},
				{"AtfRepositoryUrl", "https://fake-gitlab.com"},
				{"AtfUserName", "FakeUser"}
			});
		}

		[Test]
		public void ScriptTask1Execute_Saves_Repositories(){
			PWrapper process = new PWrapper(UserConnection);
			AtfFindAppRepositoriesOnServerMethodsWrapperTestWrapper sut = new AtfFindAppRepositoriesOnServerMethodsWrapperTestWrapper(process);
			sut.Execute();
		}
	}
	
	
	public class AtfFindAppRepositoriesOnServerMethodsWrapperTestWrapper : AtfFindAppRepositoriesOnServerMethodsWrapper {

		public AtfFindAppRepositoriesOnServerMethodsWrapperTestWrapper(Process process)
			: base(process){ }

		
		public void Execute(){
			
			var method = typeof(AtfFindAppRepositoriesOnServerMethodsWrapper)
				.GetMethod("ScriptTask1Execute", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			method.Invoke(this, new object[] { null });
		}
	}
	
	public class PWrapper : Process {
		public PWrapper(UserConnection userConnection)
			: base(userConnection){
		}
		public override ProcessSchema ProcessSchema => new ProcessSchema(UserConnection.ProcessSchemaManager);
	} 
}