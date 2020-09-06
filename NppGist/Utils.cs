using ServiceStack.Text;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NppGist.JsonMapping;

namespace NppGist
{
    public class Utils
    {
        public static readonly HttpMethod PatchHttpMethod = new HttpMethod("PATCH");

        static Utils()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            JsConfig.IncludeNullValuesInDictionaries = true;
        }

        public static string SendRequest(string url, string token = null, HttpMethod method = null,
            JsonGistObject obj = null, int timeout = 5000)
            => SendRequestAsync(url, token, method, obj, timeout).Result;

        public static T SendJsonRequest<T>(string url, string token = null, HttpMethod method = null,
            JsonGistObject obj = null, int timeout = 5000)
            => SendJsonRequestAsync<T>(url, token, method, obj, timeout).Result;

        public static async Task<string> SendRequestAsync(string url, string token = null, HttpMethod method = null,
            JsonGistObject obj = null, int timeout = 5000)
        {
            var response = await MakeRequest(url, token, method, obj, timeout).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<T> SendJsonRequestAsync<T>(string url, string token = null, HttpMethod method = null,
            JsonGistObject obj = null, int timeout = 5000)
        {
            var response = await MakeRequest(url, token, method, obj, timeout).ConfigureAwait(false);
            var result = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return JsonSerializer.DeserializeFromStream<T>(result);
        }

        public static Task<HttpResponseMessage> MakeRequest(string url, string token = null, HttpMethod method = null,
            JsonGistObject obj = null, int timeout = 5000)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url);

            if ((method == HttpMethod.Post || method?.Method == PatchHttpMethod.Method) && obj != null)
            {
                var str = JsonSerializer.SerializeToString(obj);
                requestMessage.Content = new StringContent(str);
            }

            var client = new HttpClient
            {
                BaseAddress = new Uri(Main.ApiUrl),
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };
            var headers = client.DefaultRequestHeaders;
            headers.UserAgent.Add(new ProductInfoHeaderValue("NppGist", "1.0"));
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                headers.Authorization = new AuthenticationHeaderValue("Token", token);
            }

            return client.SendAsync(requestMessage);
        }

        public static string GetSafeFilename(string filename)
        {
            return string.Join("-", filename.Split(Lists.InvalidFilenameCharacters));
        }

        public static bool IsFilenameSafe(string filename)
        {
            return filename.IndexOfAny(Lists.InvalidFilenameCharacters) == -1;
        }

        public static string ReplaceNotCharactersOnHyphens(string filename)
        {
            var result = new StringBuilder(filename.Length);
            bool lastCharIsHyphen = true;
            for (int i = 0; i < filename.Length; i++)
            {
                if (IsLatinLetterDigitOrUnderline(filename[i]))
                {
                    result.Append(char.ToLower(filename[i]));
                    lastCharIsHyphen = false;
                }
                else if (!lastCharIsHyphen && IsPunctuationOrSymbol(filename[i]))
                {
                    result.Append('-');
                    lastCharIsHyphen = true;
                }
            }

            int lastLatinOrDigitInd = 0;
            for (int i = result.Length - 1; i >= 0; i--)
            {
                if (IsLatinLetterDigitOrUnderline(result[i]))
                {
                    lastLatinOrDigitInd = i + 1;
                    break;
                }
            }
            result.Remove(lastLatinOrDigitInd, result.Length - lastLatinOrDigitInd);

            return result.ToString();
        }

        public static bool IsLatinLetterDigitOrUnderline(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9' || c == '_';
        }

        public static bool IsPunctuationOrSymbol(char c)
        {
            return char.IsPunctuation(c) || char.IsSymbol(c);
        }
    }
}
