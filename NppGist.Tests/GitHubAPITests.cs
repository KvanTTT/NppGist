using NUnit.Framework;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NppGist.Tests
{
	[TestFixture]
	public class GitHubAPITests
	{
		string AccessToken;
		
		[SetUp]
		public void Init()
		{
			// Enter your access token with gist scope in App.config
			AccessToken = ConfigurationManager.AppSettings["AccessToken"];
		}

		[Test]
		public void CheckToken()
		{
			Dictionary<string, string> responseHeaders;
			var response = Utils.SendRequest(string.Format("https://api.github.com/user?access_token={0}", AccessToken), out responseHeaders);
			var user = JsonSerializer.DeserializeFromString<User>(response);
			string scopes;
			Assert.AreEqual("gist", responseHeaders.TryGetValue("X-OAuth-Scopes", out scopes));
		}

		[Test]
		public void GetGists()
		{
			var gists = Utils.SendJsonRequest<List<Gist>>(string.Format("https://api.github.com/user?access_token={0}", AccessToken));
		}
	}
}
