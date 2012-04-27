using System;
using System.IO;
using System.Linq;
using Duplicity.Filtering.IgnoredFiles.FnMatch;

namespace Duplicity.Filtering.IgnoredFiles.GitIgnore
{
    /// <summary>
    /// Matches the patterns that do not contain a slash /, treating them as a shell glob pattern but checks for a match against the pathname without leading directories.
    /// </summary>
    internal sealed class FileMatcher : IMatcher
    {
        private readonly FileNameMatcher _matcher;        

        public FileMatcher(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentNullException("pattern");

            if (pattern.StartsWith("!"))
                throw new ArgumentException("Negated patterns should not be used", "pattern");

            if (pattern.Contains(Path.DirectorySeparatorChar))
                throw new ArgumentException("Matches files only, not directories", "pattern");
    
            _matcher = new FileNameMatcher(pattern, null);
        }

        public bool IsMatch(FileSystemChange change)
        {
            _matcher.Reset();
            _matcher.Append(ExtractFileOrDirectoryNameOnly(change));            
            
            return _matcher.IsMatch();
        }

        private static string ExtractFileOrDirectoryNameOnly(FileSystemChange change)
        {
            switch (change.Source)
            {
                case FileSystemSource.Directory:
                    return Path.GetDirectoryName(change.FileOrDirectoryPath);

                case FileSystemSource.File:
                    return Path.GetFileName(change.FileOrDirectoryPath);

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}