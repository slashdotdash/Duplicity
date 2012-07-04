using System.IO;
using System.Linq;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class AggregateMultipleFileChangesInQueue : WithAFileSystemChangeQueue
    {
        private Establish context = () => Input
            .FileCreated(@"File.txt")
            .FileChanged(@"File.txt")
            .FileChanged(@"File.txt")
            .FileChanged(@"File.txt");    

        private Because of = () => FileSystemChanges();

        private It should_include_single_change_for_file = () => Queue.Pending.Count().ShouldEqual(1);
        private It should_aggregate_changes = () => PendingAt(0).Change.ShouldEqual(WatcherChangeTypes.Created);
    }
}