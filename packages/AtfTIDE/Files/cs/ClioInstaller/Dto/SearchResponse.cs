using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AtfTIDE.ClioInstaller.Dto {
	
	public class SearchResponse {

		#region Properties: Public

		[JsonPropertyName("@context")]
		public Context Context { get; set; }

		[JsonPropertyName("data")]
		public IEnumerable<Data> Data { get; set; }

		[JsonPropertyName("totalHits")]
		public int TotalHits { get; set; }

		#endregion

	}

	public class Context {

		#region Properties: Public

		[JsonPropertyName("@base")]
		public string Base { get; set; }

		[JsonPropertyName("@vocab")]
		public string Vocab { get; set; }

		#endregion

	}

	public class Data {

		#region Properties: Public

		[JsonPropertyName("@id")]
		public string Id { get; set; }

		[JsonPropertyName("@type")]
		public string Type { get; set; }

		[JsonPropertyName("authors")]
		public string[] authors { get; set; }

		[JsonPropertyName("description")]
		public string description { get; set; }

		[JsonPropertyName("id")]
		public string PackageIdId { get; set; }

		[JsonPropertyName("licenseUrl")]
		public string LicenseUrl { get; set; }

		[JsonPropertyName("owners")]
		public IEnumerable<string> Owners { get; set; }

		[JsonPropertyName("packageTypes")]
		public IEnumerable<PackageType> PackageTypes { get; set; }

		[JsonPropertyName("projectUrl")]
		public Uri ProjectUrl { get; set; }

		[JsonPropertyName("registration")]
		public Uri Registration { get; set; }

		[JsonPropertyName("summary")]
		public string Summary { get; set; }

		[JsonPropertyName("tags")]
		public IEnumerable<string> Tags { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("totalDownloads")]
		public int TotalDownloads { get; set; }

		[JsonPropertyName("verified")]
		public bool Verified { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("versions")]
		public IEnumerable<Version> Versions { get; set; }

		[JsonPropertyName("vulnerabilities")]
		public object[] Vulnerabilities { get; set; }

		#endregion

	}

	public class PackageType {

		#region Properties: Public

		[JsonPropertyName("name")]
		public string Name { get; set; }

		#endregion

	}

	public class Version {

		#region Properties: Public

		[JsonPropertyName("@id")]
		public string Id { get; set; }

		[JsonPropertyName("downloads")]
		public int Downloads { get; set; }

		[JsonPropertyName("version")]
		public string VVersion { get; set; }

		#endregion

	}
}