using System.Collections.Generic;
using Duplicity.Collections;

namespace Duplicity.Specifications.Duplicating.Queue
{
    public sealed class ListProducer : IProduceFileSystemChanges
    {
        private readonly Queue<FileSystemChange> _queue = new Queue<FileSystemChange>();

        public ListProducer(IEnumerable<FileSystemChange> changes)
        {
            changes.Each(_queue.Enqueue);
        }

        public bool IsEmpty
        {
            get { return _queue.Count == 0; }
        }

        public bool Add(FileSystemChange change)
        {
            _queue.Enqueue(change);
            return true;
        }

        public bool TryTake(out FileSystemChange change)
        {
            if (_queue.Count == 0)
            {
                change = default(FileSystemChange);
                return false;
            }

            change = _queue.Dequeue();
            return true;
        }
    }
}
