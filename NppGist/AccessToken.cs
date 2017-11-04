using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace NppGist
{
    public class AccessToken
    {
        private const string Salt = "ds4.3Sr*rei837=984uhkjhsA-w4oq8";
        private static byte[] Vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };

        public static string EncryptToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return "";

            var key = CalculateComputerId();

            var encryptor = (new RijndaelManaged()).CreateEncryptor(key, Vector);
            var bytes = Transform(Encoding.UTF8.GetBytes(token), encryptor);

            return Convert.ToBase64String(bytes).Replace('=', '_');
        }

        public static string DecryptToken(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            var key = CalculateComputerId();

            var decryptor = (new RijndaelManaged()).CreateDecryptor(key, Vector);
            var bytes = Transform(Convert.FromBase64String(str.Replace('_', '=')), decryptor);

            return Encoding.UTF8.GetString(bytes);
        }

        private static byte[] CalculateComputerId()
        {
            var result = new StringBuilder();
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "select SerialNumber from Win32_BaseBoard");
                var collection = searcher.Get();
                foreach (var MotherboardInfo in collection)
                {
                    result.Append(MotherboardInfo["SerialNumber"].ToString());
                    break;
                }
            }
            catch
            {
            }
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "select ProcessorId FROM Win32_Processor");
                var collection = searcher.Get();
                foreach (ManagementObject queryObj in collection)
                {
                    result.Append(queryObj["ProcessorId"]);
                    break;
                }
            }
            catch
            {
            }

            result.Append(Salt);
            var hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(result.ToString())).Take(16);

            return hash.ToArray();
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            byte[] result = null;
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    cryptoStream.Write(buffer, 0, buffer.Length);
                result = stream.ToArray();
            }
            return result;
        }
    }
}
