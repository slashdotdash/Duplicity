using System.Collections.Concurrent;
using System.Linq;
using Duplicity.Specifications.Duplicating.Handlers;
using Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Tasks
{
    public abstract class WithAFileSystemChangeBlockingCollection
    {
        protected static InputBuilder Input;
        protected static BlockingCollection<FileSystemChange> Collection;
        protected static TestHandlerFactory Handler;

        protected Establish Context = () =>
        {
            Input = new InputBuilder();
            Handler = new TestHandlerFactory();
        };

        protected static void HandleFileSystemChanges()
        {
            Collection = Input.ToBlockingCollection();
            new FileSystemChangeConsumer(Collection, Handler);
            
            Wait.Until(() => Collection.Count == 0);
        }

        protected static FileSystemChange ExecutedAt(int index)
        {
            return Handler.Executed.ElementAt(index).Change;
        }
    }
}