using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Duplicity.Collections;

namespace Duplicity.Filtering
{
    /// <summary>
    /// Filter out any file or directory changes that occur before the file or directory (or one of its ancestors) are deleted.
    /// </summary>
    internal sealed class IgnoreChangesBeforeDeletionsFilter : IObservable<IList<FileSystemChange>>
    {
        private readonly IObservable<IList<FileSystemChange>> _observable;

        /// <summary>
        /// Requires an observable sequence of buffers to sucessfully group/merge changes.        
        /// </summary>
        /// <example>
        ///     observable.Buffer(TimeSpan.FromSeconds(10)).PrioritizeFileSystemChanges().Subscribe(...);
        /// </example>
        public IgnoreChangesBeforeDeletionsFilter(IObservable<IList<FileSystemChange>> observable)
        {
            _observable = observable;
        }
   
        public IDisposable Subscribe(IObserver<IList<FileSystemChange>> observer)
        {
            return _observable.Select(PrioritizeChanges)
                .Subscribe(observer);
        }

        private static IList<FileSystemChange> PrioritizeChanges(IList<FileSystemChange> source)
        {
            if (source.Count == 1) return source;

            var agregatedChanges = new AggregatedChangeVisitor();
            var directory = new DirectoryTree();

            source.Each(directory.Add);
            directory.Accept(agregatedChanges);

            return agregatedChanges.Changes;
        }

        internal sealed class AggregatedChangeVisitor : IComplexNodeVisitor<DirectoryTree>
        {
            private readonly IList<FileSystemChange> _changes = new List<FileSystemChange>();

            public void Visit(DirectoryTree node)
            {
                if (node == null) return;
                if (node.Change == null) return;
                
                _changes.Add(node.Value.Change);
            }

            public IList<FileSystemChange> Changes
            {
                get { return _changes; }
            }
        }
    }
}