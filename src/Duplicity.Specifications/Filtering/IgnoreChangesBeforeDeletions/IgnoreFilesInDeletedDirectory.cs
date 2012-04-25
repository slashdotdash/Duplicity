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

        private It should_filter_all_changes_as_all_deleted = () => Output.Count.ShouldEqual(0);
    }
}