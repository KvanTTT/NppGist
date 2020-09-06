using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NppGist.JsonMapping
{
    public class ClientError : JsonGistObject
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "errors", IsRequired = false, EmitDefaultValue = false)]
        public List<Error> Errors;
    }

    public class Error : JsonGistObject
    {
        [DataMember(Name = "resource")]
        public string Resource { get; set; }

        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
