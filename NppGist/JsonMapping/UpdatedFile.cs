using System.Runtime.Serialization;

namespace NppGist.JsonMapping
{
    public class UpdatedFile : JsonGistObject
    {
        [DataMember(Name = "filename", IsRequired = false, EmitDefaultValue = false)]
        public string Filename { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
