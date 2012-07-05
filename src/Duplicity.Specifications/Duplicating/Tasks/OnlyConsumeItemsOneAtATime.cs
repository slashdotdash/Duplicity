using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Tasks
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class OnlyConsumeItemsOneAtATime : WithABlockingFileSystemChangeConsumer
    {
        private Establish context = () =>
                                        {
                                            Input.FileCreated("Created.txt");
                                            ConsumeFileSystemChangesBlocking();
                                        };

        private Because of = () => ProduceAndConsume(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Created, "Other.txt"));

        private It should_not_consume_second_change = () => Producer.IsEmpty.ShouldBeFalse();
    }

    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class PerpetuallyConsumeChanges : WithAFileSystemChangeConsumer
    {
        private Establish context = () =>
                                        {
                                            Input.FileCreated("Created.txt");
                                            ConsumeFileSystemChanges();
                                        };

        private Because of = () => ProduceAndConsume(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Created, "Other.txt"));

        private It should_consume_second_change = () => Producer.IsEmpty.ShouldBeTrue();
    }
}