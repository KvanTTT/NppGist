using NUnit.Framework;
using ServiceStack.Text;
using System.Collections.Generic;

namespace NppGist.Tests
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void ParseUser()
        {
            var userJsonString = TestUtils.ReadDataFile("user-full.json");
            var userFull = JsonSerializer.DeserializeFromString<User>(userJsonString);
            Assert.AreEqual("Ivan Kochurkin", userFull.Name);
            Assert.AreEqual(1150330, userFull.Id);

            userJsonString = TestUtils.ReadDataFile("user.json");
            var user = JsonSerializer.DeserializeFromString<User>(userJsonString);
        }

        [Test]
        public void ParseGists()
        {
            var gistsString = TestUtils.ReadDataFile("gists.json");
            var gists = JsonSerializer.DeserializeFromString<List<Gist>>(gistsString);
        }

        [Test]
        public void CreateGist()
        {
            var creatingGist = new UpdatedGist
            {
                Description = "the description for this gist",
                Public = true,
                Files = new Dictionary<string, UpdatedFile>
                {
                    { "file1.txt", new UpdatedFile { Content = "String file contents" } }
                }
            };

            var str = JsonSerializer.SerializeToString(creatingGist);
        }

        [Test]
        public void ParseGist()
        {
            var gistString = TestUtils.ReadDataFile("gist-full.json");
            var gist = JsonSerializer.DeserializeFromString<Gist>(gistString);
        }
    }
}
