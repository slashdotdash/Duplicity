using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
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
            observable.Subscribe(OnFileSystemChange);
        }

        public IEnumerable<FileSystemChange> Pending
        {
            get { return _queue.ToArray(); }
        }

        private void OnFileSystemChange(FileSystemChange change)
        {
            lock (_padlock)
            {
                if (_queue.Count == 0)
                {
                    _queue.AddLast(change);
                }
                else
                {
                    CancelAnyPending(change);
                    Enqueue(change);
                }
            }
        }

        /// <summary>
        /// Cancel any pending changes made obsolete by the given change.
        /// </summary>
        /// <param name="change">Latest file system change</param>
        private void CancelAnyPending(FileSystemChange change)
        {
            foreach (var pending in PendingChanges())
            {
                if (ShouldBeRemoved(pending.Value, change))
                {
                    _queue.Remove(pending);
                }
            }
        }

        /// <summary>
        /// Add the pending change to the end of the queue.
        /// </summary>
        private void Enqueue(FileSystemChange change)
        {
            foreach (var current in PendingChanges())
            {
                var pending = current.Value;

                if (pending.IsSameFileOrDirectory(change))
                {
                    var resultantChange = FileSystemChangeStateMachine.Get(pending.Change.ToFileSystemChangeType(), change.Change.ToFileSystemChangeType());

                    _queue.Remove(current);

                    // Latest change makes pending change obsolete so remove it and return
                    if (resultantChange.Is(FileSystemChangeType.None))
                    {
                        return;
                    }

                    // Update with aggregated change to append to the queue
                    change = new FileSystemChange(change.Source, resultantChange.ToWatcherChangeTypes(), change.FileOrDirectoryPath);
                    break;
                }               
            }

            _queue.AddLast(change);
        }        

        private IEnumerable<LinkedListNode<FileSystemChange>> PendingChanges()
        {
            var current = _queue.First;

            while (current != null)
            {
                var next = current.Next;

                yield return current;

                current = next;
            }
        }

        /// <summary>
        /// Ignore pending changes for files or directories contained with a deleted parent directory
        /// </summary>
        private static bool ShouldBeRemoved(FileSystemChange pending, FileSystemChange change)
        {
            if (IsDeletedDirectory(pending))
            {
                if (IsContainedWithin(change, pending))
                {
                    return true;
                }
            }

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