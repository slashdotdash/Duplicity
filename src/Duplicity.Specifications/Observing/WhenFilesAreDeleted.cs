using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(DirectoryObservable))]
    public sealed class WhenFilesAreDeleted : WithAnObservableDirectory
    {
        private static string _deletedFilename;

        private Establish Context = () =>
        {
            _deletedFilename = Path.Combine(SourceDirectory, "Deleted File.txt");
            File.Create(_deletedFilename).Close();
            CreateDirectoryObservable();
        };

        private Because of = () => File.Delete(_deletedFilename);

        private It should_notify_file_changed = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.File);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Deleted);
        private It should_include_deleted_file_path = () => Change.FileOrDirectoryName.ShouldEqual(_deletedFilename);
    } 
}