using System.IO;

namespace Duplicity
{
    public static class TempPath
    {
        /// <summary>
        /// Creates a uniquely named directory in the current system's temporary folder and returns the full path of that directory.
        /// </summary>
        public static string GetTempDirectoryName()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(path);

            return path;
        }
    }
}