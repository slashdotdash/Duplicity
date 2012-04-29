using System.Collections.Generic;
using System.IO;
using Duplicity.Filtering;
using Duplicity.Filtering.IgnoredFiles.GitIgnore;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoredFiles.GitIgnore
{
    public abstract class WithAGitIgnoreFilter
    {
        protected static IList<string> Patterns;

        private static IFileSystemChangeFilter _filter;

        protected Establish Context = () =>
        {
        };

        protected static void WithIgnoredPattern(string pattern)
        {
            WithIgnoredPatterns(pattern);
        }

        protected static void WithIgnoredPatterns(params string[] patterns)
        {
            _filter = new GitIgnoreParser(patterns).CreateFilter();
        }

        protected static void ShouldIgnoreChangedFile(params string[] path)
        {
           IsIgnoredFile(Path.Combine(path)).ShouldBeTrue();
        }

        protected static void ShouldNotIgnoreChangedFile(params string[] path)
        {            
            IsIgnoredFile(Path.Combine(path)).ShouldBeFalse();
        }

        protected static void ShouldIgnoreChangedDirectory(params string[] path)
        {
            IsIgnoredDirectory(Path.Combine(path)).ShouldBeTrue();
        }

        protected static void ShouldNotIgnoreChangedDirectory(params string[] path)
        {
            IsIgnoredDirectory(Path.Combine(path)).ShouldBeFalse();
        }

        private static bool IsIgnoredFile(string path)
        {
            return _filter.Filter(new FileSystemChange(FileSystemSource.File, WatcherChangeTypes.Created, path));
        }

        private static bool IsIgnoredDirectory(string path)
        {
            return _filter.Filter(new FileSystemChange(FileSystemSource.Directory, WatcherChangeTypes.Created, path));
        }
    }
}