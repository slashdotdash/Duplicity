using System.Linq;
using Duplicity.Specifications.Duplicating.Handlers;
using Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Tasks
{
    public abstract class WithAFileSystemChangeConsumer
    {
        protected static InputBuilder Input;
        protected static TestHandlerFactory Handler;
        protected static IProduceFileSystemChanges Producer;
        protected static IConsumeFileSystemChanges Consumer;

        protected Establish Context = () =>
        {
            Input = new InputBuilder();
            Handler = new TestHandlerFactory();
            Consumer = new FileSystemChangeConsumer(Handler);
        };

        protected static void ConsumeFileSystemChanges()
        {
            Producer = Input.ToProducer();
            Consumer.Consume(Producer);
            
            Wait.Until(() => Producer.IsEmpty);
        }

        protected static FileSystemChange ExecutedAt(int index)
        {
            return Handler.Executed.ElementAt(index).Change;
        }
    }
}