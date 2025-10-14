using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleGit.Dto;
using ConsoleGit.Services;
using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

/// <summary>
/// Returns the current (active) branch name.
/// </summary>
public class GetActiveBranchCommand(CommandLineArgs args, IWebSocketLogger logger)
	: BaseRepositoryCommand(args, logger)
{
	public override ErrorOr<Success> Execute() {
		var branchesOrError = InitializedRepository.ListLocalBranches();
		if (branchesOrError.IsError) {
			return branchesOrError.Errors;
		}

		Branch? active = branchesOrError.Value.FirstOrDefault(b => b.IsCurrentRepositoryHead);
		string name = active == null
			? string.Empty
			: (active.FriendlyName.StartsWith("origin/", StringComparison.Ordinal)
				? active.FriendlyName.Substring("origin/".Length)
				: active.FriendlyName);

		var payload = new ActiveBranchResponse { Name = name };
		string json = JsonSerializer.Serialize(payload, AppJsonSerializerContext.Default.ActiveBranchResponse);
		Console.Out.WriteLine(json);
		return Result.Success;
	}
}

public class ActiveBranchResponse
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}