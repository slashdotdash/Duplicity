using System;
using System.IO;
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
        private const char GitIgnorePathSeparator = '/';

        public PathMatcher(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException("pattern");
            if (pattern.StartsWith("!")) throw new ArgumentException("Negated patterns should not be used", "pattern");

            // Strip leading forward slash from pattern as matched paths don't start with the path separator character
            if (pattern.StartsWith(GitIgnorePathSeparator + string.Empty))
                pattern = pattern.Substring(1);

            _matcher = new FileNameMatcher(pattern, GitIgnorePathSeparator);
        }

        public bool IsMatch(FileSystemChange change)
        {
            _matcher.Reset();
            _matcher.Append(AdaptToPattern(change.FileOrDirectoryPath));

            return _matcher.IsMatch();
        }

        /// <summary>
        /// .gitignore patterns are specified using the Unix path separator character (/).
        /// We need to replace the current environment directory separator charactor with a forward slash.
        /// </summary>
        private static string AdaptToPattern(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, GitIgnorePathSeparator);
        }
    }
}