using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NppGist
{
	[DataContract]
	public class UpdatedGist
	{
		[DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "public", IsRequired = false, EmitDefaultValue = false)]
		public bool Public { get; set; }

		[DataMember(Name = "files")]
		public Dictionary<string, UpdatedFile> Files { get; set; }
	}
}
