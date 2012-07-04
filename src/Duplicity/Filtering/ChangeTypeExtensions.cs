using System;
using System.IO;
using System.Linq;
using Duplicity.Filtering.Aggregation;

namespace Duplicity.Filtering
{
    internal static class ChangeTypeExtensions
    {
        public static FileSystemChangeType ToFileSystemChangeType(this WatcherChangeTypes source)
        {
            switch (source)
            {
                case WatcherChangeTypes.Created: return FileSystemChangeType.Created;
                case WatcherChangeTypes.Changed: return FileSystemChangeType.Changed;
                case WatcherChangeTypes.Deleted: return FileSystemChangeType.Deleted;
            }

            throw new NotSupportedException();
        }

        public static WatcherChangeTypes ToWatcherChangeTypes(this FileSystemChangeType source)
        {
            switch (source)
            {
                case FileSystemChangeType.Created: return WatcherChangeTypes.Created;
                case FileSystemChangeType.Changed: return WatcherChangeTypes.Changed;
                case FileSystemChangeType.Deleted: return WatcherChangeTypes.Deleted;
            }

            throw new NotSupportedException();
        }

        public static bool IsDeletedDirectory(this FileSystemChange change)
        {
            return change.Source == FileSystemSource.Directory && change.Change == WatcherChangeTypes.Deleted;
        }

        public static bool Contains(this FileSystemChange parent, FileSystemChange child)
        {
            if (parent.FileOrDirectoryPath == child.FileOrDirectoryPath) return false;

            var parentDirs = parent.Directories();
            var childDirs = child.Directories();

            for (var i = 0; i < childDirs.Length; i++)
            {
                if (childDirs[i] != parentDirs[i]) 
                    return false;
            }

            return true;
        }

        private static string[] Directories(this FileSystemChange change)
        {
            var dirs = change.FileOrDirectoryPath.Split(Path.DirectorySeparatorChar);

            switch (change.Source)
            {
                case FileSystemSource.Directory:
                    return dirs;

                case FileSystemSource.File:                    
                    return dirs.Take(dirs.Length - 1).ToArray();

                default:
                    throw new NotSupportedException();
            }            
        }
    }
}