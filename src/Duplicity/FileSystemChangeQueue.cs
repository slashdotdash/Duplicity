using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Duplicity.Filtering.Aggregation;
using Duplicity.Filtering;

namespace Duplicity
{
    /// <summary>
    /// Queue of file system changes that uses a linked list internally to allow cancellation and removal of pending changes.
    /// Implements IProducerConsumerCollection to allow thread-safe producer/consumer usage.
    /// </summary>
    public sealed class FileSystemChangeQueue : IProducerConsumerCollection<FileSystemChange>
    {
        private readonly object _padlock = new object();

        private readonly LinkedList<FileSystemChange> _queue = new LinkedList<FileSystemChange>();

        public bool TryAdd(FileSystemChange change)
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

            return true;
        }

        public bool TryTake(out FileSystemChange item)
        {
            lock (_padlock)
            {
                if (_queue.Count == 0)
                {
                    item = default(FileSystemChange);
                    return false;
                }

                item = _queue.First.Value;
                _queue.RemoveFirst();
            }

            return true;
        }

        public IEnumerable<FileSystemChange> Pending
        {
            get { return ToArray(); }
        }

        public IEnumerator<FileSystemChange> GetEnumerator()
        {
            lock (_padlock)
            {
                var list = new List<FileSystemChange>(_queue.Count);
                list.AddRange(_queue);
                return list.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            lock (_padlock) { ((ICollection)_queue).CopyTo(array, index); }
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public object SyncRoot
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public void CopyTo(FileSystemChange[] array, int index)
        {
            lock (_padlock) { _queue.CopyTo(array, index); }
        }        

        public FileSystemChange[] ToArray()
        {
            FileSystemChange[] copy;
            lock (_padlock) { copy = _queue.ToArray(); }
            return copy;
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
            return change.IsDeletedDirectory() && change.Contains(pending);
        }        
    }
}