using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenFilesAreDeleted : WithAnObservableDirectory
    {        
        private Establish Context = () =>
        {
            File.Create(Path.Combine(SourceDirectory, "Deleted File.txt")).Close();
            CreateFileSystemObservable();
        };

        private Because of = () => File.Delete(Path.Combine(SourceDirectory, "Deleted File.txt"));

        private It should_notify_file_changed = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.File);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Deleted);
        private It should_include_deleted_file_path = () => Change.FileOrDirectoryPath.ShouldEqual("Deleted File.txt");
    } 
}