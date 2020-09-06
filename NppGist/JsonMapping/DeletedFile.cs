using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NppGist.JsonMapping
{
    public class DeletedFile : JsonGistObject
    {
        [DataMember(Name = "files")]
        public Dictionary<string, string> Files { get; set; }
    }
}