using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenFilesAreCreated : WithAnObservableDirectory
    {
        private Establish context = () => CreateFileSystemObservable();
        private Because of = () => File.Create(Path.Combine(SourceDirectory, "New File.txt")).Close();

        private It should_notify_new_file_created = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.File);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Created);
        private It should_include_changed_file_path = () => Change.FileOrDirectoryPath.ShouldEqual("New File.txt");
    }    
}