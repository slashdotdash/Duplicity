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
        };

        protected static void ApplyChanges()
        {
            Queue = new FileSystemChangeQueue(Input.ToObservable());
        }

        protected static FileSystemChange PendingAt(int index)
        {
            return Queue.Pending.ElementAt(index);
        }
    }
}