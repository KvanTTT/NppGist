using NUnit.Framework;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace NppGist.Tests
{
	[TestFixture]
	public class JsonTests
	{
		[Test]
		public void ParseUser()
		{
			var userJsonString = System.IO.File.ReadAllText(@"..\..\Data\user full.json");
			var userFull = JsonSerializer.DeserializeFromString<User>(userJsonString);
			Assert.AreEqual("Ivan Kochurkin", userFull.Name);
			Assert.AreEqual(1150330, userFull.Id);
			Assert.AreEqual(new DateTime(2011, 10, 25, 14, 34, 49), userFull.CreatedAt);

			userJsonString = System.IO.File.ReadAllText(@"..\..\Data\user.json");
			var user = JsonSerializer.DeserializeFromString<User>(userJsonString);
		}

		[Test]
		public void ParseGists()
		{
			var gistsString = System.IO.File.ReadAllText(@"..\..\Data\gists.json");
			var gists = JsonSerializer.DeserializeFromString<List<Gist>>(gistsString);

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Gist>));
			using (var stream = new FileStream(@"..\..\Data\gists.json", FileMode.Open))
			{
				var gists2 = (List<Gist>)serializer.ReadObject(stream);
			}
		}

		[Test]
		public void CreateGist()
		{
			var creatingGist = new UpdatedGist
			{
				Description = "the description for this gist",
				Public = true,
				Files = new Dictionary<string, UpdatedFile>()
				{
					{
						"file1.txt", new UpdatedFile { Content = "String file contents" }
					}
				}
			};

			var str = JsonSerializer.SerializeToString<UpdatedGist>(creatingGist);
		}

		[Test]
		public void ParseGist()
		{
			var gistString = File.ReadAllText(@"..\..\Data\gist full.json");
			var gist = JsonSerializer.DeserializeFromString<Gist>(gistString);

			gist = JsonSerializer.DeserializeFromString<Gist>("sdf");
		}
	}
}
