using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenDirectoriesAreDeleted : WithAnObservableDirectory
    {
        private static string _deletedDirectory;

        private Establish context = () =>
                                        {
                                            _deletedDirectory = Path.Combine(SourceDirectory, "Deleted Directory");
                                            Directory.CreateDirectory(_deletedDirectory);
                                            CreateFileSystemObservable();
                                        };
        private Because of = () => Directory.Delete(_deletedDirectory);

        private It should_notify_new_directory_created = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.Directory);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Deleted);
        private It should_include_changed_file_path = () => Change.FileOrDirectoryName.ShouldEqual(_deletedDirectory);
    }
}