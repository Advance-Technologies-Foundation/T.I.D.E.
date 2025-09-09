using System;
using System.Runtime.Serialization;

namespace AtfTIDE.WebServices{
    [DataContract]
    public class DiscardFileChangesDto{
        [DataMember(Name = "files")] public string[] Files { get; set; }

        [DataMember(Name = "repositoryId")] public Guid RepositoryId { get; set; }
    }
}