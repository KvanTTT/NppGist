using NUnit.Framework;
using ServiceStack.Text;
using System.Collections.Generic;
using NppGist.JsonMapping;

namespace NppGist.Tests
{
    [TestFixture]
    public class GitHubAPITests
    {
        [Test]
        public void GetUser()
        {
            var response = Utils.SendRequestAsync("https://api.github.com/users/KvanTTT").Result;
            var user = JsonSerializer.DeserializeFromString<User>(response);
        }

        [Test]
        public void GetGists()
        {
            var gists = Utils.SendJsonRequestAsync<List<Gist>>("https://api.github.com/gists").Result;
            Assert.Greater(gists.Count, 0);
        }
    }
}
