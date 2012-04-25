using System;
using System.IO;
using System.Linq;
using Duplicity.Collections;

namespace Duplicity.Filtering
{
    internal sealed class DirectoryTree : ComplexTreeNode<DirectoryTree>
    {
        public string Name { get; set; }
        public FileSystemChange Change { get; set; }
    }

    internal static class DirectoryTreeExtensions
    {
        public static void Add(this DirectoryTree source, FileSystemChange change)
        {
            var path = change.FileOrDirectoryPath.Split(Path.DirectorySeparatorChar);
            var parent = path.Take(path.Length - 1).Aggregate(source, GetOrCreateDirectory);

            var target = Get(parent, path.Last());
            if (target == null)
            {
                target = Create(parent, path.Last());
                target.Change = change;
            }
            else if (target.Change == null)
            {
                target.Change = change;
            }
            else
            {
                var resultantChange = FileSystemChangeStateMachine.Get(Convert(target.Value.Change.Change), Convert(change.Change));
                switch (resultantChange)
                {
                    case FileSystemChangeType.None:
                        target.Parent.Children.Remove(target);
                        return;

                    default:
                        target.Change = new FileSystemChange(change.Source, Convert(resultantChange), change.FileOrDirectoryPath);
                        break;
                }
            }

            // When deleting a directory, delete all contained changes
            if (IsDeletingADirectory(target.Change))
            {
                target.Children.Clear();
            }
        }

        private static bool IsDeletingADirectory(FileSystemChange change)
        {
            return change.Source == FileSystemSource.Directory && change.Change == WatcherChangeTypes.Deleted;
        }

        private static FileSystemChangeType Convert(WatcherChangeTypes source)
        {
            switch (source)
            {
                case WatcherChangeTypes.Created: return FileSystemChangeType.Created;
                case WatcherChangeTypes.Changed: return FileSystemChangeType.Changed;
                case WatcherChangeTypes.Deleted: return FileSystemChangeType.Deleted;
            }

            throw new NotSupportedException();
        }

        private static WatcherChangeTypes Convert(FileSystemChangeType source)
        {
            switch (source)
            {
                case FileSystemChangeType.Created: return WatcherChangeTypes.Created;
                case FileSystemChangeType.Changed: return WatcherChangeTypes.Changed;
                case FileSystemChangeType.Deleted: return WatcherChangeTypes.Deleted;
            }

            throw new NotSupportedException();
        }

        private static DirectoryTree GetOrCreateDirectory(ComplexTreeNode<DirectoryTree> parent, string directory)
        {
            return Get(parent, directory) ?? Create(parent, directory);
        }

        private static DirectoryTree Get(ComplexTreeNode<DirectoryTree> parent, string directory)
        {
            return (DirectoryTree)parent.Children.FirstOrDefault(child => child.Value.Name == directory);
        }

        private static DirectoryTree Create(ComplexTreeNode<DirectoryTree> parent, string directory)
        {
            return parent.Children.Add(new DirectoryTree { Name = directory });
        }       
    }    
}