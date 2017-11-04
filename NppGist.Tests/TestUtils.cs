using System.IO;
using System.Runtime.CompilerServices;

namespace NppGist.Tests
{
    public static class TestUtils
    {
        public static string ProjectDir { get; private set; }

        static TestUtils()
        {
            InitProjectDir();
        }

        public static string ReadDataFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(ProjectDir, "Data", fileName));
        }

        private static void InitProjectDir([CallerFilePath] string thisFilePath = null)
        {
            ProjectDir = Path.GetDirectoryName(thisFilePath);
        }
    }
}
