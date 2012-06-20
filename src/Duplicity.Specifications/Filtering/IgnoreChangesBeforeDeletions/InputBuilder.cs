using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    public sealed class InputBuilder
    {
        private readonly IList<FileSystemChange> _changes = new List<FileSystemChange>();

        public InputBuilder DirectoryCreated(string path)
        {
            return Change(FileSystemSource.Directory, WatcherChangeTypes.Created, path);
        }

        public InputBuilder DirectoryDeleted(string path)
        {
            return Change(FileSystemSource.Directory, WatcherChangeTypes.Deleted, path);
        }

        public InputBuilder FileCreated(string path)
        {
            return Change(FileSystemSource.File, WatcherChangeTypes.Created, path);
        }

        public InputBuilder FileChanged(string path)
        {
            return Change(FileSystemSource.File, WatcherChangeTypes.Changed, path);
        }

        public InputBuilder FileDeleted(string path)
        {
            return Change(FileSystemSource.File, WatcherChangeTypes.Deleted, path);
        }

        private InputBuilder Change(FileSystemSource source, WatcherChangeTypes type, string path)
        {
            _changes.Add(new FileSystemChange(source, type, path));
            return this;
        }

        public IObservable<FileSystemChange> ToObservable()
        {
            return _changes.ToObservable();
        }

        public IObservable<IList<FileSystemChange>> ToBufferedObservable()
        {
            return _changes.ToObservable().Buffer(_changes.Count);
        }
    }
}