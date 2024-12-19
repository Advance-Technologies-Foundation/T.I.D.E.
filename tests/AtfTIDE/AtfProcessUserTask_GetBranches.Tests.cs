using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Terrasoft.Common;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Core.Process.Configuration;

namespace AtfTIDE.Tests {
	[TestFixture]
	[MockSettings(RequireMock.All)]
	public class AtfProcessUserTask_GetBranches_Tests : BaseMarketplaceTestFixture{

	
		private AtfProcessUserTask_GetBranchesWrapper _sut;
		IConsoleGit consoleGitMock = Substitute.For<IConsoleGit>();

		protected override void SetUp(){
			base.SetUp();
			ClassFactory.RebindWithFactoryMethod<IConsoleGit>(() => consoleGitMock, "AtfTIDE.ConsoleGit");
			_sut = new AtfProcessUserTask_GetBranchesWrapper(UserConnection);
		}

		[Test]
		public void Execute_WhenCalled_ShouldCallInternalExecute() {
			//Arrange
			consoleGitMock.Execute(Arg.Any<ConsoleGitArgs>())
			.Returns(new ConsoleGitResult(0,
			"{\r\n \"branches\":\r\n[{ \"name\": \"master\" },\r\n { \"name\": \"develop\" }]\r\n}"));
			MockEntitySchemaWithColumns("AtfRepository", new Dictionary<string, DataValueType> {
		{ "Name", DataValueType.Text },
		{ "AtfName", DataValueType.Text },
		{ "AtfRepositoryUrl", DataValueType.Text },
		{ "AtfUserName", DataValueType.Text },
		{ "AtfAccessToken", DataValueType.Text }
		});
			SetUpTestData("AtfRepository", new Dictionary<string, object> { { "Name", "TestRepository" } });
			//Act
			_sut.Execute();
			//Assert
			_sut.Branches.Count.Should().Be(2);
			CompositeObjectList<CompositeObject> branches = new CompositeObjectList<CompositeObject>(); 
			foreach (var item in _sut.Branches) {
				branches.Add(item as CompositeObject);
			}
			branches.Any(b => b["Name"].ToString() == "master").Should().BeTrue();
			branches.Any(b => b["Name"].ToString() == "develop").Should().BeTrue();
		}
	}

	public class AtfProcessUserTask_GetBranchesWrapper: ATFProcessUserTask_GetBranches {

		public AtfProcessUserTask_GetBranchesWrapper(UserConnection userConnection)
			: base(userConnection){ }

		public void Execute(){
			base.InternalExecute(null);
		}
	}
}