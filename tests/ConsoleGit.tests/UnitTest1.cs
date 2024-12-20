using System.Text.Json;
using ConsoleGit.Commands;
using ErrorOr;
using FluentAssertions;

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
		var sut = new GetBranchesCommand(args);
		
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