using System;
using System.IO;
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
    }
}