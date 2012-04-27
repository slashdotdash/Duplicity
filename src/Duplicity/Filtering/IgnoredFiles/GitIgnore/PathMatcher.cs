using System;
using Duplicity.Filtering.IgnoredFiles.FnMatch;

namespace Duplicity.Filtering.IgnoredFiles.GitIgnore
{
    /// <summary>
    /// Match patterns as a shell glob suitable for consumption by fnmatch, but wildcards in the pattern will not match a / in the pathname.
    /// </summary>
    /// <example>
    /// For example, "Documentation/*.html" matches "Documentation/git.html" but not "Documentation/ppc/ppc.html". 
    /// A leading slash matches the beginning of the pathname; for example, "/*.c" matches "cat-file.c" but not "mozilla-sha1/sha1.c".
    /// </example>
    internal sealed class PathMatcher : IMatcher
    {
        private readonly FileNameMatcher _matcher;        

        public PathMatcher(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException("pattern");
            if (pattern.StartsWith("!")) throw new ArgumentException("Negated patterns should not be used", "pattern");
            
            _matcher = new FileNameMatcher(pattern, null);
        }

        public bool IsMatch(FileSystemChange change)
        {
            _matcher.Reset();
            _matcher.Append(change.FileOrDirectoryPath);

            return _matcher.IsMatch();
        }
    }
}