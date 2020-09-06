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
            var response = new GitHubService(null).SendRequestAsync("users/KvanTTT").Result;
            var user = JsonSerializer.DeserializeFromString<User>(response);
        }

        [Test]
        public void GetGists()
        {
            var gists = new GitHubService(null).SendJsonRequestAsync<List<Gist>>("gists").Result;
            Assert.Greater(gists.Count, 0);
        }
    }
}
