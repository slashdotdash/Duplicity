using System.IO;

namespace Duplicity
{
    public enum FileSystemSource
    {
        Directory,
        File
    }

    public sealed class FileSystemChange
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

        #region Equality Members
        public bool Equals(FileSystemChange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._source, _source) && Equals(other._change, _change) && Equals(other._fileOrDirectoryPath, _fileOrDirectoryPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (FileSystemChange)) return false;
            return Equals((FileSystemChange) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _source.GetHashCode();
                result = (result*397) ^ _change.GetHashCode();
                result = (result*397) ^ _fileOrDirectoryPath.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(FileSystemChange left, FileSystemChange right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileSystemChange left, FileSystemChange right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}