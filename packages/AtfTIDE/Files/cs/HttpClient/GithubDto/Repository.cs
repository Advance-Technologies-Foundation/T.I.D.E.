using System.Text.Json.Serialization;
using AtfTIDE.HttpClient.Converters;
using JetBrains.Annotations;

namespace AtfTIDE.HttpClient.GithubDto {
	public class Repository {
		
		[JsonPropertyName("id")]
		public uint Id { get; set; }
		
		[JsonPropertyName("node_id")]
		public string NodeId { get; set; }
		
		[JsonPropertyName("name")]
		public string Name { get; set; }
		
		[JsonPropertyName("description")]
		[CanBeNull]
		public string Description { get; set; }
		
		[JsonPropertyName("full_name")]
		public string FullName { get; set; }
		
		[JsonPropertyName("private")]
		public bool Private { get; set; }
		
		[JsonPropertyName("url")]
		public string Url { get; set; }
		
		[JsonPropertyName("clone_url")]
		public string CloneUrl { get; set; }
		
		[JsonPropertyName("custom_properties")]
		[CanBeNull]
		public CustomProperties CustomProperties { get; set; }
	}
	
	
	/// <summary>
	/// This class represents custom properties for a GitHub repository.
	/// We can use this to clearly define which repositories are Clio workspaces.
	/// </summary>
	public class CustomProperties {

		[JsonPropertyName("clio-workspace")]
		[JsonConverter(typeof(JsonStringBoolConverter))]
		public bool ClioWorkspace { get; set; }
	}
}
