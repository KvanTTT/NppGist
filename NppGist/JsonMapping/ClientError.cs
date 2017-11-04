using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NppGist
{
    [DataContract]
    public class ClientError
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "errors", IsRequired = false, EmitDefaultValue = false)]
        public List<Error> Errors;
    }

    [DataContract]
    public class Error
    {
        [DataMember(Name = "resource")]
        public string Resource { get; set; }

        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
