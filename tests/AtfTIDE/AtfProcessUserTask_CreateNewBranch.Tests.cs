using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Process.Configuration;

namespace AtfTIDE.Tests {
	[TestFixture]
	[MockSettings(RequireMock.All)]
	public class AtfProcessUserTask_CreateNewBranch_Tests : BaseMarketplaceTestFixture{

	
		private AtfProcessUserTask_CreateNewBranchWrapper _sut;

		protected override void SetUp(){
			base.SetUp();
			_sut = new AtfProcessUserTask_CreateNewBranchWrapper(UserConnection);
		}

		[Test]
		public void Execute_WhenCalled_ShouldCallInternalExecute(){
			//Arrange
		
			//Act
			_sut.Execute();
		
			//Assert
			_sut.IsError.Should().BeTrue();
			_sut.ErrorMessage.Should().Be("Error");
		}

	}

	public class AtfProcessUserTask_CreateNewBranchWrapper: AtfProcessUserTask_CreateNewBranch {

		public AtfProcessUserTask_CreateNewBranchWrapper(UserConnection userConnection)
			: base(userConnection){ }

		public void Execute(){
			base.InternalExecute(null);
		}
	}
}