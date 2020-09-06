using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NppGist.JsonMapping
{
    public class UpdatedGist : JsonGistObject
    {
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "public", IsRequired = false, EmitDefaultValue = false)]
        public bool Public { get; set; }

        [DataMember(Name = "files")]
        public Dictionary<string, UpdatedFile> Files { get; set; }
    }
}
