using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleGit.Dto;

[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(WorkspaceSettings))]
public partial class AppJsonSerializerContext : JsonSerializerContext { }