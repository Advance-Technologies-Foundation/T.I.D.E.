using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleGit.Commands;

namespace ConsoleGit.Dto;

[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(WorkspaceSettings))]
[JsonSerializable(typeof(BranchesCommandResponse))]
[JsonSerializable(typeof(MyBranch))]
public partial class AppJsonSerializerContext : JsonSerializerContext { }