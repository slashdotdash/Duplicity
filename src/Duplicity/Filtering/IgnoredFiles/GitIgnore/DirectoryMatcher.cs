using System;
using System.IO;
using Duplicity.Filtering.IgnoredFiles.FnMatch;

namespace Duplicity.Filtering.IgnoredFiles.GitIgnore
{
    /// <summary>
    /// Matches patterns ending with a slash, it is removed for the purpose of the following description, but it would only find a match with a directory. 
    /// </summary>
    /// <example>
    /// For example, foo/ will match a directory foo and paths underneath it, but will not match a regular file or a symbolic link foo.
    /// </example>
    internal sealed class DirectoryMatcher : IMatcher
    {
        private readonly FileNameMatcher _matcher;

        public DirectoryMatcher(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) 
                throw new ArgumentNullException("pattern");

            if (pattern.StartsWith("!")) 
                throw new ArgumentException("Negated patterns should not be used", "pattern");

            if (pattern[pattern.Length - 1] != Path.DirectorySeparatorChar) 
                throw new ArgumentException("Matches directories only, not files", "pattern");

            _matcher = new FileNameMatcher(pattern.Substring(0, pattern.Length - 1), null);
        }

        public bool IsMatch(FileSystemChange change)
        {
            if (change.Source != FileSystemSource.Directory) return false;

            _matcher.Reset();
            _matcher.Append(change.FileOrDirectoryPath);

            return _matcher.IsMatch();
        }
    }
}