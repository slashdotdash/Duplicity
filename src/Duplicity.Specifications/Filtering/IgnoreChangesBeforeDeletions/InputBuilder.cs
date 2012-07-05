using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Duplicity.Collections;
using Duplicity.Specifications.Duplicating.Queue;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    public sealed class InputBuilder : IObservable<FileSystemChange>
    {
        private readonly IList<FileSystemChange> _changes = new List<FileSystemChange>();
        private readonly Subject<FileSystemChange> _subject = new Subject<FileSystemChange>();

        public void Publish()
        {
            _changes.Each(change => _subject.OnNext(change));
        }

        public IProduceFileSystemChanges ToProducer()
        {
            return new ListProducer(_changes);
        }

        public IObservable<IList<FileSystemChange>> ToBufferedObservable()
        {
            return _changes.ToObservable().Buffer(_changes.Count);
        }

        public IDisposable Subscribe(IObserver<FileSystemChange> observer)
        {
            return _subject.Subscribe(observer);
        }

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
    }
}