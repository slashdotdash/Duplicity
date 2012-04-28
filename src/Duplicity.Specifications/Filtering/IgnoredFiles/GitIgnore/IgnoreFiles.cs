using Duplicity.Filtering.IgnoredFiles.GitIgnore;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoredFiles.GitIgnore
{
    [Subject(typeof(FileMatcher))]
    public class IgnoreChangedFile : WithAGitIgnoreFilter
    {        
        private Establish context = () => WithIgnoredPattern("*.html");

        private It should_ignore_file = () => ShouldIgnoreChangedFile("test.html");
        private It should_ignore_file_in_directory = () => ShouldIgnoreChangedFile(@"Documentation\foo.html");
        private It should_ignore_file_in_directory_unix = () => ShouldIgnoreChangedFile(@"Documentation/foo.html");        
    }
}