using System.Text;

namespace NppGist
{
    public class Utils
    {
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
            foreach (var c in filename)
            {
                if (IsLatinLetterDigitOrUnderline(c))
                {
                    result.Append(char.ToLower(c));
                    lastCharIsHyphen = false;
                }
                else if (!lastCharIsHyphen && IsPunctuationOrSymbol(c))
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

        private static bool IsLatinLetterDigitOrUnderline(char c) =>
            c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9' || c == '_';

        private static bool IsPunctuationOrSymbol(char c) => char.IsPunctuation(c) || char.IsSymbol(c);
    }
}
