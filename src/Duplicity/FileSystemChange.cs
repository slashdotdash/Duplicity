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
        private readonly string _fileOrDirectoryName;        

        public FileSystemChange(FileSystemSource source, WatcherChangeTypes change, string fileOrDirectoryName)
        {
            _source = source;
            _change = change;
            _fileOrDirectoryName = fileOrDirectoryName;
        }

        public FileSystemSource Source
        {
            get { return _source; }
        }

        public WatcherChangeTypes Change
        {
            get { return _change; }
        }

        public string FileOrDirectoryName
        {
            get { return _fileOrDirectoryName; }
        }
    }
}