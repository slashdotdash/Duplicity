using System.Collections.Concurrent;
using System.Linq;
using Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue
{
    public abstract class WithAFileSystemChangeQueue
    {
        protected static InputBuilder Input;
        protected static FileSystemChangeQueue Queue;
        
        protected Establish Context = () =>
        {
            Input = new InputBuilder();
            Queue = new FileSystemChangeQueue();

            new BlockingCollection<FileSystemChange>(Queue).AddFromObservable(Input, true);
        };

        protected static void FileSystemChanges()
        {
            Input.Publish();
        }

        protected static FileSystemChange PendingAt(int index)
        {
            return Queue.Pending.ElementAt(index);
        }
    }
}