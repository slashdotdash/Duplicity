using System.IO;

namespace Duplicity.Specifications
{
    internal static class TempPath
    {
        /// <summary>
        /// Returns a unique temporary directory name.
        /// </summary>
        public static string GetTempDirectoryName()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            //var path = Path.GetTempFileName();
            //File.Delete(path);
            Directory.CreateDirectory(path);

            return path;
        }
    }
}
