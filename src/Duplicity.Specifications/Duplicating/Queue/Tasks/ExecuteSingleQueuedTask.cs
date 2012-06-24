using System.IO;
using System.Linq;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue.Tasks
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class ExecuteSingleQueuedTask : WithAFileSystemChangeQueue
    {
        private Establish context = () => Input.FileCreated(@"New File.txt");

        private Because of = () => ApplyChanges();

        private It should_queue_all_changes = () => Queue.Pending.Count().ShouldEqual(2);

        private It should_include_directory_change_source = () => PendingAt(0).Source.ShouldEqual(FileSystemSource.Directory);
        private It should_include_directory_type_of_change = () => PendingAt(0).Change.ShouldEqual(WatcherChangeTypes.Created);
        private It should_include_directory_changed_file_path = () => PendingAt(0).FileOrDirectoryPath.ShouldEqual("New Directory");
    }
}