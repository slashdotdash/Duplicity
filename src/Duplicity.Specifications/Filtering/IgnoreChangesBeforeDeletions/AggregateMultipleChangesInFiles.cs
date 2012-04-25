using System.IO;
using Duplicity.Filtering;
using Machine.Specifications;
using Duplicity.Specifications.SpecExtensions;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    [Subject(typeof(IgnoreChangesBeforeDeletionsFilter))]
    public sealed class AggregateMultipleChangesInFiles : WithFilter
    {
        private Establish context = () => Input
                                              .FileChanged(@"Dir\Foo.txt")
                                              .FileChanged(@"Dir\Foo.txt")
                                              .FileChanged(@"Dir\Foo.txt")
                                              .FileChanged(@"Dir\Foo.txt");

        private Because of = () => Filter();

        private It should_filter_duplicate_file_changes_to_single_change = () => Output.Count.ShouldEqual(1);
        private It should_include_file_change_once = () => OutputAt(0).ShouldEqual(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Changed, @"Dir\Foo.txt"));
    }
}