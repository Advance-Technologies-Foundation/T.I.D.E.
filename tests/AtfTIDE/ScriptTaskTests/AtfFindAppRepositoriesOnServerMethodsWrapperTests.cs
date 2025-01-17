using System;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Process;

namespace AtfTIDE.Tests.ScriptTaskTests {
	
	[TestFixture]
	[MockSettings(RequireMock.All)]
	public class AtfFindAppRepositoriesOnServerMethodsWrapperTests : BaseMarketplaceTestFixture {
		
		[Test]
		public void ScriptTask1Execute_Saves_Repositories(){
			PWrapper process = new PWrapper(UserConnection);
			var sut = new AtfFindAppRepositoriesOnServerMethodsWrapperTestWrapper(process);
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