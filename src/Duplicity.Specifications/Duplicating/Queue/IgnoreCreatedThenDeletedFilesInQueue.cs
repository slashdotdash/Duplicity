using System.Linq;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating.Queue
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class IgnoreCreatedThenDeletedFilesInQueue : WithAFileSystemChangeQueue
    {
        private Establish context = () => Input
            .FileCreated(@"File.txt")
            .FileChanged(@"File.txt")
            .FileDeleted(@"File.txt");                          

        private Because of = () => FileSystemChanges();

        private It should_ignore_created_then_deleted_files = () => Queue.Pending.Count().ShouldEqual(0);
    }

    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class IgnoreCreatedThenDeletedDirectoriesInQueue : WithAFileSystemChangeQueue
    {
        private Establish context = () => Input
            .DirectoryCreated(@"Dir")
            .DirectoryDeleted(@"Dir");

        private Because of = () => FileSystemChanges();

        private It should_ignore_created_then_deleted_directories = () => Queue.Pending.Count().ShouldEqual(0);
    }
}