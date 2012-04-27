using Duplicity.Filtering.IgnoredFiles;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoredFiles
{
    [Subject(typeof(FileMatcher))]
    public class IgnoreFiles
    {
        private static FileMatcher _matcher;
        private Establish context = () => _matcher = new FileMatcher("foot.txt");

        private It should_match_single_file = () => _matcher.IsMatch("foo.txt").ShouldBeTrue();
    }
}
