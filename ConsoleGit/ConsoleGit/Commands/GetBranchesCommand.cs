using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleGit.Dto;
using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

/// <summary>
/// Pushes the specified branch to the remote repository.
/// </summary>
/// <param name="args">Arguments</param>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-branch"/>
public class GetBranchesCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {
	public override ErrorOr<Success> Execute(){
		
		ErrorOr<IEnumerable<Branch>> branches = InitializedRepository.ListLocalBranches();
		
		if(branches.IsError){
			return Error.Failure("COULD_NOT_GET_BRANCHES","Could not get branches from repository");
		}
		BranchesCommandResponse model = new() {
			Branches = branches.Value
			                   .Select(b => b.FriendlyName.TrimStart("origin/".ToCharArray()))
			                   .Distinct()
			                   .Where(v=> !v.StartsWith("HEAD"))
			                   .Select(v => new MyBranch { Name = v })
			                   .ToList()
		};

		string json = JsonSerializer.Serialize(model, AppJsonSerializerContext.Default.BranchesCommandResponse);
		Console.Out.WriteLine(json);
		return Result.Success;
	}

}

public class BranchesCommandResponse
{
	[JsonPropertyName("branches")]
	public List<MyBranch> Branches
	{
		get; set;
	}
}

public class MyBranch
{
	[JsonPropertyName("name")]
	public string Name
	{
		get; set;
	}
}
