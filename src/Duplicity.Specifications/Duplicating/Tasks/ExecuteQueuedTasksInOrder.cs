using System.IO;
using Machine.Specifications;
using Duplicity.Specifications.SpecExtensions;

namespace Duplicity.Specifications.Duplicating.Tasks
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class ExecuteQueuedTasksInOrder : WithAFileSystemChangeConsumer
    {
        private Establish context = () => Input
            .DirectoryCreated("Dir")
            .FileCreated(@"Dir\New File.txt")
            .DirectoryDeleted("Deleted")
            .FileChanged("Changed.txt")
            .FileDeleted("Deleted.txt");

        private Because of = () => ConsumeFileSystemChanges();

        private It should_handle_first_change = () => ExecutedAt(0).ShouldEqual(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Created, "Dir"));
        private It should_handle_second_change = () => ExecutedAt(1).ShouldEqual(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Created, @"Dir\New File.txt"));
        private It should_handle_third_change = () => ExecutedAt(2).ShouldEqual(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Deleted, "Deleted"));
        private It should_handle_fourth_change = () => ExecutedAt(3).ShouldEqual(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Changed, "Changed.txt"));
        private It should_handle_fifth_change = () => ExecutedAt(4).ShouldEqual(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Deleted, "Deleted.txt"));

        private It should_have_no_more_changes = () => Producer.IsEmpty.ShouldBeTrue();
    }
}