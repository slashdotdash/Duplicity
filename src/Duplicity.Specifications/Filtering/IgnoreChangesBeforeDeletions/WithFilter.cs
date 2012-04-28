using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Duplicity.Filtering;
using Duplicity.Filtering.Aggregation;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    public abstract class WithFilter
    {
        private static IgnoreChangesBeforeDeletionsFilter _filter;
        private static IDisposable _subscription;

        private static InputBuilder _input;
        protected static List<FileSystemChange> Output;

        protected Establish Context = () =>
                                          {
                                              _input = new InputBuilder();
                                              Output = new List<FileSystemChange>();
                                          };

        protected static InputBuilder Input
        {
            get { return _input; }
        }

        protected static FileSystemChange OutputAt(int index)
        {
            return Output.ElementAt(index);
        }

        protected static void Filter()
        {
            _filter = new IgnoreChangesBeforeDeletionsFilter(_input.ToObservable());
            _subscription = _filter.Subscribe(change => Output.AddRange(change));
        }

        protected Cleanup Dispose = () => _subscription.Dispose();
    }

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

        public IObservable<IList<FileSystemChange>> ToObservable()
        {
            return _changes.ToObservable().Buffer(_changes.Count);
        }        
    }
}