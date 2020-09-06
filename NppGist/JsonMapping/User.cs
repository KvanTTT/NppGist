using System;
using System.Runtime.Serialization;

namespace NppGist.JsonMapping
{
    public class User : JsonGistObject
    {
        [DataMember(Name = "login")]
        public string Login { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "avatar_url")]
        public string AvatarUrl { get; set; }

        [DataMember(Name = "gravatar_id")]
        public string GravatarId { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "html_url")]
        public string HtmlUrl { get; set; }

        [DataMember(Name = "followers_url")]
        public string FollowersUrl { get; set; }

        [DataMember(Name = "following_url")]
        public string FollowingUrl { get; set; }

        [DataMember(Name = "gists_url")]
        public string GistsUrl { get; set; }

        [DataMember(Name = "starred_url")]
        public string StarredUrl { get; set; }

        [DataMember(Name = "subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [DataMember(Name = "organizations_url")]
        public string OrganizationsUrl { get; set; }

        [DataMember(Name = "repos_url")]
        public string ReposUrl { get; set; }

        [DataMember(Name = "events_url")]
        public string EventsUrl { get; set; }

        [DataMember(Name = "received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "site_admin")]
        public bool SiteAdmin { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "company")]
        public string Company { get; set; }

        [DataMember(Name = "blog")]
        public string Blog { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "hireable")]
        public bool Hireable { get; set; }

        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name = "public_repos")]
        public int PublicRepos { get; set; }

        [DataMember(Name = "public_gists")]
        public int PublicGists { get; set; }

        [DataMember(Name = "followers")]
        public int Followers { get; set; }

        [DataMember(Name = "following")]
        public int Following { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
