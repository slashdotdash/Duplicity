using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Observing
{
    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenFilesAreModified : WithAnObservableDirectory
    {
        private Establish Context = () =>
        {
            File.Create(Path.Combine(SourceDirectory, "Modified File.txt")).Close();
            CreateFileSystemObservable();
        };

        private Because of = () =>
        {
            File.AppendAllText(Path.Combine(SourceDirectory, "Modified File.txt"), "Some appended text");
            Wait.Until(() => Changes.Count == 1);
        };

        private It should_notify_file_changed = () => Changes.Count.ShouldEqual(1);
        private It should_include_change_source = () => Change.Source.ShouldEqual(FileSystemSource.File);
        private It should_include_type_of_change = () => Change.Change.ShouldEqual(WatcherChangeTypes.Changed);
        private It should_include_changed_file_path = () => Change.FileOrDirectoryPath.ShouldEqual("Modified File.txt");
    } 
}