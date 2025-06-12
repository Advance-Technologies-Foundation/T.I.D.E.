using System.Text.Json;
using ConsoleGit.Commands;
using ConsoleGit.Services;
using ErrorOr;
using FluentAssertions;
using NSubstitute;

namespace ConsoleGit.tests;

[TestFixture]
public class Tests {

	
	[Test]
	public void GetBranches_ReturnsBranches(){
		//Arrange
		CommandLineArgs args = new () {
			RepoDir = @"C:\Projects\Workspaces\TIDE",
			Command = "GetBranches"
		};
		using StringWriter sw = new ();
		Console.SetOut(sw);
		var logger = Substitute.For<IWebSocketLogger>();
		var sut = new GetBranchesCommand(args, logger);
		
		// Act
		ErrorOr<Success> result = sut.Execute();
		
		//Assert
		result.IsError.Should().BeFalse();
		string consoleOutput = sw.ToString();
		
		BranchesCommandResponse? model = JsonSerializer
			.Deserialize<BranchesCommandResponse>(consoleOutput);
		model.Should().NotBeNull();
		model.Branches.Should().HaveCountGreaterThan(2);
	}
}