using System;
using System.Collections.Generic;
using System.Linq;
using Duplicity.Filtering.Aggregation;
using Duplicity.Filtering;

namespace Duplicity
{
    /// <summary>
    /// Queue of file system changes that uses a linked list internally to allow cancellation and removal of pending changes.
    /// </summary>
    public sealed class FileSystemChangeQueue : IProduceFileSystemChanges, IDisposable
    {
        private readonly object _padlock = new object();

        private readonly LinkedList<FileSystemChange> _queue = new LinkedList<FileSystemChange>();

        private readonly IConsumeFileSystemChanges _consumer;

        public FileSystemChangeQueue(IObservable<FileSystemChange> observable, IConsumeFileSystemChanges consumer)
        {
            _consumer = consumer;

            observable.Subscribe(change => Add(change));
        }

        public bool IsEmpty
        {
            get { lock (_padlock) { return _queue.Count == 0; } }
        }

        public bool Add(FileSystemChange change)
        {
            lock (_padlock)
            {
                if (_queue.Count == 0)
                {
                    AddLast(change);                    
                }
                else
                {
                    CancelAnyPending(change);
                    Enqueue(change);
                }
            }

            return true;
        }

        public bool TryTake(out FileSystemChange change)
        {
            lock (_padlock)
            {
                if (_queue.Count == 0)
                {
                    change = default(FileSystemChange);
                    return false;
                }

                change = _queue.First.Value;
                _queue.RemoveFirst();
            }

            return true;
        }

        public IEnumerable<FileSystemChange> Pending
        {
            get { return _queue.ToList(); }
        }

        public void Dispose()
        {
            
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

            AddLast(change);
        }        

        /// <summary>
        /// Adds the given change to the end of the queue and notifies consumer that at least one change is pending.
        /// </summary>
        private void AddLast(FileSystemChange change)
        {
            _queue.AddLast(change);

            // Notify consumer of the change
            _consumer.Consume(this);
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
            return change.IsDeletedDirectory() && change.Contains(pending);
        }        
    }
}