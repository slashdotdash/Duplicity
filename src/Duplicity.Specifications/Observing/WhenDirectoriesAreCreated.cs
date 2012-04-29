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
        private It should_include_changed_file_path = () => Change.FileOrDirectoryPath.ShouldEqual("New Directory");
    }

    [Subject(typeof(FileSystemObservable))]
    public sealed class WhenSubDirectoriesAreCreated : WithAnObservableDirectory
    {
        private Establish context = () => CreateFileSystemObservable();
        private Because of = () =>
        {
            Directory.CreateDirectory(Path.Combine(SourceDirectory, "New Directory"));
            Directory.CreateDirectory(Path.Combine(SourceDirectory, "New Directory", "Sub Directory"));
            Wait.Until(() => Changes.Count == 2);
        };

        private It should_notify_new_directories_created = () => Changes.Count.ShouldEqual(2);
        private It should_include_directory_change = () => ChangeAt(0).ShouldEqual(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Created, "New Directory"));
        private It should_include_sub_directory_change = () => ChangeAt(1).ShouldEqual(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Created, @"New Directory\Sub Directory"));
    } 
}