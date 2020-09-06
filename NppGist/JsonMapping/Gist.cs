using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NppGist.JsonMapping
{
    public class Gist : JsonGistObject
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "forks_url")]
        public string ForksUrl { get; set; }

        [DataMember(Name = "commits_url")]
        public string CommitsUrl { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "git_pull_url")]
        public string GitPullUrl { get; set; }

        [DataMember(Name = "git_push_url")]
        public string GitPushUrl { get; set; }

        [DataMember(Name = "html_url")]
        public string HtmlUrl { get; set; }

        [DataMember(Name = "files")]
        public Dictionary<string, GistFile> Files { get; set; }

        [DataMember(Name = "public")]
        public bool Public { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "comments_url")]
        public string CommentsUrl { get; set; }
    }
}
