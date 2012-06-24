using System.IO;
using System.Linq;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class IgnoreQueuedChangesContainedWithinDeletedDirectories : WithAFileSystemChangeQueue
    {
        private Establish context = () => Input
            .FileCreated(@"Dir\New.txt")
            .FileChanged(@"Dir\Changed.txt")
            .FileDeleted(@"Dir\File.txt")
            .DirectoryDeleted("Dir");

        private Because of = () => ApplyChanges();

        private It should_ignore_changes_contained_within_deleted_directory = () => Queue.Pending.Count().ShouldEqual(1);
        private It should_include_deleted_directory_change = () => PendingAt(0).ShouldEqual(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Deleted, "Dir"));
    }
}