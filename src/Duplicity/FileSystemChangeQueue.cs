using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Duplicity.Filtering.Aggregation;
using Duplicity.Filtering;

namespace Duplicity
{
    /// <summary>
    /// Queue of file system changes that uses a linked list internally to allow filtering/removal of pending changes
    /// </summary>
    public sealed class FileSystemChangeQueue : IDisposable
    {
        private readonly object _padlock = new object();

        private readonly LinkedList<FileSystemChange> _queue = new LinkedList<FileSystemChange>();

        private readonly CompositeDisposable _subscription = new CompositeDisposable();
        
        public FileSystemChangeQueue(IObservable<FileSystemChange> observable)
        {
            observable.Subscribe(change =>
                                     {
                                         lock (_padlock)
                                         {
                                             CancelAnyPending(change);
                                             Enqueue(change);
                                         }
                                     });
        }

        public IEnumerable<FileSystemChange> Pending
        {
            get { return _queue.ToArray(); }
        }

        /// <summary>
        /// Add the pending change to the end of the queue.
        /// </summary>
        private void Enqueue(FileSystemChange change)
        {
            _queue.AddLast(change);
        }

        /// <summary>
        /// Cancel any pending changes made obsolete by the given change.
        /// </summary>
        /// <param name="change"></param>
        private void CancelAnyPending(FileSystemChange change)
        {
            if (_queue.Count == 0) return;

            var current = _queue.First;

            while (current != null)
            {
                var next = current.Next;

                if (ShouldBeRemoved(current.Value, change))
                    _queue.Remove(current);

                current = next;
            }
        }

        private static bool ShouldBeRemoved(FileSystemChange pending, FileSystemChange change)
        {
            //if (pending.IsSameFileOrDirectory(change))
            //{
            //    var resultantChange = FileSystemChangeStateMachine.Get(pending.Change.ToFileSystemChangeType(), change.Change.ToFileSystemChangeType());

            //    return resultantChange == FileSystemChangeType.None;
            //}

            //// Ignore pending changes for files or directories contained with a deleted parent directory
            //if (IsDeletedDirectory(pending))
            //{
            //    return IsContainedWithin(change, pending);
            //}

            return false;
        }
        
        private static bool IsDeletedDirectory(FileSystemChange change)
        {
            return change.Source == FileSystemSource.Directory && change.Change == WatcherChangeTypes.Deleted;
        }

        private static bool IsContainedWithin(FileSystemChange parent, FileSystemChange child)
        {
            return false;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}