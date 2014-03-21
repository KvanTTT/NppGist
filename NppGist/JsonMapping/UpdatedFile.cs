using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NppGist
{
	[DataContract]
	public class UpdatedFile
	{
		[DataMember(Name = "filename", IsRequired = false, EmitDefaultValue = false)]
		public string Filename { get; set; }

		[DataMember(Name = "content")]
		public string Content { get; set; }
	}
}
