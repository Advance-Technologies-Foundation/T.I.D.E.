using System.Text.Json.Serialization;

namespace ConsoleGit.Dto;


public record WorkspaceSettings(
    [property:JsonPropertyName("Packages")]string[] Packages,
    [property:JsonPropertyName("ApplicationVersion")]string ApplicationVersion
);

