using System.Runtime.Serialization;

namespace NppGist
{
    [DataContract]
    public class GistFile
    {
        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "raw_url")]
        public string RawUrl { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }
    }
}
