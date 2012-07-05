using System.Linq;
using Duplicity.Specifications.Duplicating.Tasks;
using Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue
{
    public abstract class WithAFileSystemChangeQueue
    {
        protected static InputBuilder Input;
        protected static FileSystemChangeQueue Queue;
        protected static NullConsumer Consumer;

        protected Establish Context = () =>
        {
            Input = new InputBuilder();
            Consumer = new NullConsumer();
            Queue = new FileSystemChangeQueue(Input, Consumer);
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