using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenDirectoriesAreCreated : WithAnObservableDirectory
    {
        private Establish context = () => CreateFileSystemObservable();
        private Because of = () => Directory.CreateDirectory(Path.Combine(SourceDirectory, "New Directory"));

        private It should_notify_new_directory_created = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.Directory);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Created);
        private It should_include_changed_file_path = () => Change.FileOrDirectoryName.ShouldEqual(Path.Combine(SourceDirectory, "New Directory"));
    }    
}