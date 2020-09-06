using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NppGist.JsonMapping;
using ServiceStack.Text;

namespace NppGist
{
    public class GitHubService : IDisposable
    {
        public static readonly HttpMethod PatchHttpMethod = new HttpMethod("PATCH");

        private readonly HttpClient httpClient;

        public string Token { get; }

        static GitHubService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            JsConfig.IncludeNullValuesInDictionaries = true;
        }

        public GitHubService(string token)
        {
            Token = token;

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com/"),
                Timeout = TimeSpan.FromMilliseconds(5000)
            };

            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.Add(new ProductInfoHeaderValue("NppGist", "1.0"));
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                headers.Authorization = new AuthenticationHeaderValue("Token", token);
            }
        }

        public async Task<string> SendRequestAsync(string url, HttpMethod method = null,
            JsonGistObject obj = null)
        {
            var response = await SendRequest(url, method, obj).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> SendJsonRequestAsync<T>(string url, HttpMethod method = null,
            JsonGistObject obj = null)
        {
            var response = await SendRequest(url, method, obj).ConfigureAwait(false);
            var result = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return JsonSerializer.DeserializeFromStream<T>(result);
        }

        public Task<HttpResponseMessage> SendRequest(string url, HttpMethod method = null, JsonGistObject obj = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url);

            if ((method == HttpMethod.Post || method?.Method == PatchHttpMethod.Method) && obj != null)
            {
                var str = JsonSerializer.SerializeToString(obj);
                requestMessage.Content = new StringContent(str);
            }

            return httpClient.SendAsync(requestMessage);
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}