using System.IO;
using System.Linq;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class WhenChangesAreQueued : WithAFileSystemChangeQueue
    {
        private Establish context = () => Input
            .DirectoryCreated("New Directory")
            .FileCreated(@"New Directory\New File.txt");

        private Because of = () => FileSystemChanges();

        private It should_queue_all_changes = () => Queue.Pending.Count().ShouldEqual(2);

        private It should_include_directory_change_source = () => PendingAt(0).Source.ShouldEqual(FileSystemSource.Directory);
        private It should_include_directory_type_of_change = () => PendingAt(0).Change.ShouldEqual(WatcherChangeTypes.Created);
        private It should_include_directory_changed_file_path = () => PendingAt(0).FileOrDirectoryPath.ShouldEqual("New Directory");

        private It should_include_file_change_source = () => PendingAt(1).Source.ShouldEqual(FileSystemSource.File);
        private It should_include_file_type_of_change = () => PendingAt(1).Change.ShouldEqual(WatcherChangeTypes.Created);
        private It should_include_file_changed_file_path = () => PendingAt(1).FileOrDirectoryPath.ShouldEqual(@"New Directory\New File.txt");
    }
}