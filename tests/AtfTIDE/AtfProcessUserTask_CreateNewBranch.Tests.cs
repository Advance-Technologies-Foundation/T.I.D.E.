using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Core.Process.Configuration;

namespace AtfTIDE.Tests {
	[TestFixture]
	[MockSettings(RequireMock.All)]
	public class AtfProcessUserTask_CreateNewBranch_Tests : BaseMarketplaceTestFixture{

		private const string RepositorySchemaName = "AtfRepository";
		private readonly IConsoleGit _consoleGitMock = Substitute.For<IConsoleGit>();
		private AtfProcessUserTask_CloneRepositoryWrapper _sut;

		protected override void SetUp(){
			base.SetUp();
			MockRepositoryEntity();
			ClassFactory.RebindWithFactoryMethod(()=> (UserConnection)UserConnection);
			ClassFactory.RebindWithFactoryMethod(()=> _consoleGitMock, "AtfTIDE.ConsoleGit");
			_sut = new AtfProcessUserTask_CloneRepositoryWrapper(UserConnection);
		}

		private void MockRepositoryEntity(){
			MockEntitySchemaWithColumns(RepositorySchemaName, new Dictionary<string, DataValueType> {
				{"AtfName", DataValueType.Text},
				{"AtfRepositoryUrl", DataValueType.Text},
				{"AtfUserName", DataValueType.Text},
				{"AtfAccessToken", DataValueType.Text}
			});
		}
		
		
		[Test]
		public void Execute_WhenCalled_ShouldCallInternalExecute(){
			//Arrange
			_sut.Repository = Guid.NewGuid();
			SetUpTestData(RepositorySchemaName, 
				filterAction: selectData => selectData.Has(_sut.Repository),
				new Dictionary<string, object> {
					{"AtfName", "TestRepo"},
					{"AtfRepositoryUrl", "https://fake-gitlab.com"},
					{"AtfUserName", "TestUser"},
					{"AtfAccessToken", "TestToken"}
			});
			
			ConsoleGitResult resultMock = new ConsoleGitResult(0, "Success");
			_consoleGitMock.Execute(Arg.Is<ConsoleGitArgs>(args => 
								args.Command == Commands.Clone &&
								args.GitUrl == "https://fake-gitlab.com" &&
								args.UserName == "TestUser" &&
								args.Password == "TestToken" &&
								args.RepoDir.EndsWith("conf\\tide\\TestRepo")
							))
							.Returns(resultMock);
			
			
			//Act
			var result = _sut.Execute();
		
			//Assert
			_sut.IsError.Should().BeFalse();
			_sut.ErrorMessage.Should().BeNull();
			result.Should().BeTrue();
		}

	}

	public class AtfProcessUserTask_CloneRepositoryWrapper: AtfProcessUserTask_CloneRepository {

		public AtfProcessUserTask_CloneRepositoryWrapper(UserConnection userConnection)
			: base(userConnection){ }
		public bool Execute() => base.InternalExecute(null);

	}
}