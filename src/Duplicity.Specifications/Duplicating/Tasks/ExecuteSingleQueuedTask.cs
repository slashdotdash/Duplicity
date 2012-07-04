using System.IO;
using System.Linq;
using Machine.Specifications;
using Duplicity.Specifications.SpecExtensions;

namespace Duplicity.Specifications.Duplicating.Tasks
{
    [Subject(typeof(FileSystemChangeQueue))]
    public sealed class ExecuteSingleQueuedTask : WithAFileSystemChangeBlockingCollection
    {
        private Establish context = () => Input.FileCreated(@"New File.txt");

        private Because of = () => HandleFileSystemChanges();

        private It should_execute_single_handler = () => Handler.Executed.Count().ShouldEqual(1);
        private It should_execute_file_created_handler = () => ExecutedAt(0).ShouldEqual(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Created, "New File.txt"));
    }
}