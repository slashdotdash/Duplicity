using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenDirectoriesAreDeleted : WithAnObservableDirectory
    {        
        private Establish context = () =>
        {
            Directory.CreateDirectory(Path.Combine(SourceDirectory, "Deleted Directory"));
            CreateFileSystemObservable();
        };
        private Because of = () => Directory.Delete(Path.Combine(SourceDirectory, "Deleted Directory"));

        private It should_notify_new_directory_created = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.Directory);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Deleted);
        private It should_include_changed_file_path = () => Change.FileOrDirectoryPath.ShouldEqual("Deleted Directory");
    }
}