using System.IO;

namespace Duplicity
{
    public enum FileSystemSource
    {
        Directory,
        File
    }

    public struct FileSystemChange
    {
        private readonly FileSystemSource _source;
        private readonly WatcherChangeTypes _change;
        private readonly string _fileOrDirectoryPath;        

        public FileSystemChange(FileSystemSource source, WatcherChangeTypes change, string fileOrDirectoryPath)
        {
            _source = source;
            _change = change;
            _fileOrDirectoryPath = fileOrDirectoryPath;
        }

        public FileSystemSource Source
        {
            get { return _source; }
        }

        public WatcherChangeTypes Change
        {
            get { return _change; }
        }

        /// <summary>
        /// Path to the changed file or directory, relative to the source observed directory
        /// </summary>
        public string FileOrDirectoryPath
        {
            get { return _fileOrDirectoryPath; }
        }
    }
}