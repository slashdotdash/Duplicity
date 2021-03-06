﻿using Duplicity.Filtering.IgnoredFiles.GitIgnore;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoredFiles.GitIgnore
{
    /// <summary>
    /// Specifications for .gitignore filtering.
    /// Examples taken from the gitignore man page and unit tests of Mono's NGit project (https://github.com/mono/ngit).
    /// </summary>
    /// <see cref="https://github.com/mono/ngit/blob/master/NGit.Test/NGit.Ignore/IgnoreMatcherTest.cs"/>
    /// <seealso cref="https://github.com/mono/ngit/blob/master/NGit.Test/NGit.Ignore/IgnoreMatcherTest.cs"/>
    /// <seealso cref="http://progit.org/book/ch2-2.html"/>
    [Subject(typeof(GitIgnoreFilter))]
    public class IgnoreFilePatternWithWildcard : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPattern("*.html");

        private It should_ignore_file_matching_wildcard = () => ShouldIgnoreChangedFile("test.html");
        private It should_ignore_file_in_directory = () => ShouldIgnoreChangedFile("Documentation", "foo.html");

        private It should_not_ignore_file_not_matching_wildcard = () => ShouldNotIgnoreChangedFile("test.xxx");
    }

    [Subject(typeof(GitIgnoreFilter))]
    public class IgnoreFilePatternWithPathComponent : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPatterns("foo/bar/*.o");

        private It should_ignore_file_in_matching_directory = () => ShouldIgnoreChangedFile("foo", "bar", "baz.o");

        private It should_not_ignore_file_in_top_level_directory = () => ShouldNotIgnoreChangedFile("test.o");
        private It should_not_ignore_file_in_another_directory = () => ShouldNotIgnoreChangedFile("a", "file", "somewhere", "down", "the", "hierarchy", "test.o");
        private It should_not_ignore_file_in_sub_directory = () => ShouldNotIgnoreChangedFile("foo", "bar", "down", "the", "hierarchy", "test.o");
    }

    [Subject(typeof(GitIgnoreFilter))]
    public class IgnoreGlobFilePattern : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPatterns("/*.c");

        private It should_ignore_file_in_matching_directory = () => ShouldIgnoreChangedFile("cat-file.c");
        private It should_not_ignore_file_in_sub_directory = () => ShouldNotIgnoreChangedFile("mozilla-sha1", "sha1.c");
    }

    [Subject(typeof(GitIgnoreFilter))]
    public class IgnoreGlobPatternWithPathComponent : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPatterns("Documentation/*.html");

        private It should_ignore_file_in_matching_directory = () => ShouldIgnoreChangedFile("Documentation", "git.html");
        private It should_not_ignore_file_in_sub_directory = () => ShouldNotIgnoreChangedFile("Documentation", "ppc", "ppc.html");
    }

    [Subject(typeof(GitIgnoreFilter))]
    public class IncludeNegatedPatternsWithWildcard : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPatterns("*.html", "!foo.html");

        private It should_ignore_file_matching_wildcard = () => ShouldIgnoreChangedFile("test.html");
        private It should_not_ignore_negated_file_match = () => ShouldNotIgnoreChangedFile("foo.html");
        private It should_not_ignore_negated_file_match_in_directory = () => ShouldNotIgnoreChangedFile("Documentation", "foo.html");
    }

    [Subject(typeof(GitIgnoreFilter))]
    public class IgnorePatternWithLetterRanges : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPattern("/[tv]est.sta[a-d]");

        private It should_ignore_file_in_range_a = () => ShouldIgnoreChangedFile("test.staa");
        private It should_ignore_file_in_range_b = () => ShouldIgnoreChangedFile("test.stab");
        private It should_ignore_file_in_range_c = () => ShouldIgnoreChangedFile("test.stac");
        private It should_ignore_file_in_range_d = () => ShouldIgnoreChangedFile("test.stac");
        private It should_ignore_file_with_alternate_inital_char = () => ShouldIgnoreChangedFile("vest.stac");

        private It should_not_ignore_file_outside_range = () => ShouldNotIgnoreChangedFile("test.stae");
        private It should_not_ignore_file_with_number = () => ShouldNotIgnoreChangedFile("test.sta9");        
    }

    [Subject(typeof (GitIgnoreFilter))]
    public class IgnorePatternWithWildcard : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPattern("*.st?");

        private It should_ignore_file = () => ShouldIgnoreChangedFile("test.stp");
        private It should_ignore_file2 = () => ShouldIgnoreChangedFile("anothertest.stg");
        private It should_ignore_file3 = () => ShouldIgnoreChangedFile("anothertest.st0");
        private It should_ignore_file4 = () => ShouldIgnoreChangedFile("anothertest.st1");

        private It should_not_ignore_file_not_matching = () => ShouldNotIgnoreChangedFile("another", "test.sta1");
    }

    [Subject(typeof (GitIgnoreFilter))]
    public class IgnoreFilesContainedInMatchingParentDirectory : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPattern("/src/ne?");

        private It should_ignore_directory = () => ShouldIgnoreChangedDirectory("src", "new");
        private It should_ignore_file_in_matching_directory = () => ShouldIgnoreChangedFile("src", "new", "a.c");
        private It should_ignore_file_in_sub_directory = () => ShouldIgnoreChangedFile("src", "new", "a", "a.c");

        private It should_not_ignore_file_not_matching = () => ShouldNotIgnoreChangedFile("src", "new.c");
    }

    [Subject(typeof(GitIgnoreFilter))]
    public class IgnoreFilesContainedInMatchingDirectory : WithAGitIgnoreFilter
    {
        private Establish context = () => WithIgnoredPattern("ne?");

        private It should_ignore_directory = () => ShouldIgnoreChangedDirectory("src", "new");
        private It should_ignore_file_in_matching_directory = () => ShouldIgnoreChangedFile("src", "new", "a.c");
        private It should_ignore_file_in_sub_directory = () => ShouldIgnoreChangedFile("src", "new", "a", "a.c");
        private It should_ignore_directory_matching = () => ShouldIgnoreChangedDirectory("neb");

        private It should_not_ignore_file_not_matching = () => ShouldNotIgnoreChangedFile("src", "new.c");
    }
}