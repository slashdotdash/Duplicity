using System.IO;
using Duplicity.Filtering.Aggregation;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    [Subject(typeof(IgnoreChangesBeforeDeletionsFilter))]
    public sealed class IgnoreChangedThenDeletedFiles : WithFilter
    {
        private Establish context = () => Input
            .DirectoryCreated(@"Dir")
            .FileCreated(@"Dir\Foo.txt")
            .FileChanged(@"Dir\Foo.txt")
            .FileDeleted(@"Dir\Foo.txt");            

        private Because of = () => Filter();

        private It should_filter_created_then_deleted_files = () => Output.Count.ShouldEqual(1);
        private It should_include_directory_created_only = () => OutputAt(0).ShouldEqual(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Created, "Dir"));
    }
}