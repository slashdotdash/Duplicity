using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(DirectoryObservable))]
    public sealed class WhenFilesAreModified : WithAnObservableDirectory
    {
        private static string _modifiedFilename;

        private Establish Context = () =>
        {
            _modifiedFilename = Path.Combine(SourceDirectory, "Modified File.txt");
            File.Create(_modifiedFilename).Close();
            CreateDirectoryObservable();
        };

        private Because of = () => File.AppendAllText(_modifiedFilename, "Some appended text");

        private It should_notify_file_changed = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.File);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Changed);
        private It should_include_changed_file_path = () => Change.FileOrDirectoryName.ShouldEqual(_modifiedFilename);
    } 
}