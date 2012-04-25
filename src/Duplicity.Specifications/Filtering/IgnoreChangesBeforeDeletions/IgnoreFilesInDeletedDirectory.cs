using Duplicity.Filtering;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    [Subject(typeof(IgnoreChangesBeforeDeletionsFilter))]
    public sealed class IgnoreFilesInDeletedDirectory : WithFilter
    {
        private Establish context = () => Input
            .DirectoryCreated(@"Dir")
            .FileCreated(@"Dir\Foo.txt")
            .FileCreated(@"Dir\Bar.txt")
            .FileCreated(@"Dir\Baz.txt")
            .DirectoryDeleted(@"Dir");

        private Because of = () => Filter();

        private It should_filter_all_changes_in_deleted_directories = () => Output.Count.ShouldEqual(0);
    }

    [Subject(typeof(IgnoreChangesBeforeDeletionsFilter))]
    public sealed class IgnoreDirectoriesInDeletedDirectory : WithFilter
    {
        private Establish context = () => Input
            .DirectoryCreated(@"Dir")
            .DirectoryCreated(@"Dir\Foo")
            .DirectoryCreated(@"Dir\Foo\Bar")
            .DirectoryCreated(@"Dir\Foo\Bar\Baz")            
            .DirectoryDeleted(@"Dir\Foo");

        private Because of = () => Filter();

        private It should_filter_all_changes_in_deleted_directories = () => Output.Count.ShouldEqual(1);
    }

    [Subject(typeof(IgnoreChangesBeforeDeletionsFilter))]
    public sealed class IgnoreDeletedDirectoriesInDeletedDirectory : WithFilter
    {
        private Establish context = () => Input
            .DirectoryDeleted(@"Foo\Bar\Baz")
            .DirectoryDeleted(@"Foo\Bar");

        private Because of = () => Filter();

        private It should_filter_all_changes_in_deleted_directories = () => Output.Count.ShouldEqual(1);
    }
}