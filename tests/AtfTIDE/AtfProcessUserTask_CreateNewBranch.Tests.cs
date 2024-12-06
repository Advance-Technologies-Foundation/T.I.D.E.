using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Process.Configuration;

namespace AtfTIDE.Tests {
	[TestFixture]
	[MockSettings(RequireMock.All)]
	public class AtfProcessUserTask_CreateNewBranch_Tests : BaseMarketplaceTestFixture{

	
		private AtfProcessUserTask_CloneRepositoryWrapper _sut;

		protected override void SetUp(){
			base.SetUp();
			_sut = new AtfProcessUserTask_CloneRepositoryWrapper(UserConnection);
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

	public class AtfProcessUserTask_CloneRepositoryWrapper: AtfProcessUserTask_CloneRepository {

		public AtfProcessUserTask_CloneRepositoryWrapper(UserConnection userConnection)
			: base(userConnection){ }

		public void Execute(){
			base.InternalExecute(null);
		}
	}
}