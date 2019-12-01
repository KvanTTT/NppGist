using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NppGist
{
    public class Utils
    {
        static Utils()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static string SendRequest(string url, WebRequestMethod method = WebRequestMethod.Get,
            Dictionary<string, string> headers = null, byte[] body = null, string contentType = "", int timeout = 5000)
        {
            return SendRequest(url, out _, method, headers, body, contentType, timeout);
        }

        public static T SendJsonRequest<T>(string url, WebRequestMethod method = WebRequestMethod.Get,
            Dictionary<string, string> headers = null, byte[] body = null, string contentType = "", int timeout = 5000)
        {
            return SendJsonRequest<T>(url, out _, method, headers, body, contentType, timeout);
        }

        public static string SendRequest(string url, out Dictionary<string, string> responseHeaders,
            WebRequestMethod method = WebRequestMethod.Get,
            Dictionary<string, string> headers = null, byte[] body = null, string contentType = "", int timeout = 5000)
        {
            var request = MakeRequest(url, method, headers, body, contentType, timeout);

            string responseString;
            responseHeaders = new Dictionary<string, string>();
            using (var response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                responseString = reader.ReadToEnd();
                foreach (var header in response.Headers)
                {
                    var key = (string)header;
                    responseHeaders.Add(key, response.Headers[key]);
                }
            }

            return responseString;
        }

        public static T SendJsonRequest<T>(string url, out Dictionary<string, string> responseHeaders,
            WebRequestMethod method = WebRequestMethod.Get,
            Dictionary<string, string> headers = null, byte[] body = null, string contentType = "", int timeout = 5000)
        {
            var request = MakeRequest(url, method, headers, body, contentType, timeout);

            T result;
            responseHeaders = new Dictionary<string, string>();
            using (var response = request.GetResponse())
            {
                result = JsonSerializer.DeserializeResponse<T>(response);
                foreach (var header in response.Headers)
                {
                    var key = (string)header;
                    responseHeaders.Add(key, response.Headers[key]);
                }
            }

            return result;
        }

        private static HttpWebRequest MakeRequest(string url, WebRequestMethod method = WebRequestMethod.Get,
            Dictionary<string, string> headers = null, byte[] body = null, string contentType = "", int timeout = 5000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToString().ToUpperInvariant();
            request.ContentType = contentType;
            request.UserAgent = "NppGist";
            request.Timeout = timeout;
            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);
            if ((method == WebRequestMethod.Post || method == WebRequestMethod.Patch) && body != null)
            {
                request.ContentType = contentType;
                request.ContentLength = body.Length;
                using (var stream = request.GetRequestStream())
                    stream.Write(body, 0, body.Length);
            }
            return request;
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
