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
            var path = change.FileOrDirectoryPath;
            var search = ExtractFileOrDirectoryNameOnly(change);

            if (IsMatch(search)) return true;

            // Go up the directory tree looking for any matches
            while (path.IndexOf(Path.DirectorySeparatorChar) > 0)
            {
                path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
                search = GetDirectoryName(path);

                if (IsMatch(search)) return true;
            }

            return false;
        }

        private bool IsMatch(string path)
        {
            _matcher.Reset();
            _matcher.Append(path);

            return _matcher.IsMatch();
        }

        private static string ExtractFileOrDirectoryNameOnly(FileSystemChange change)
        {
            switch (change.Source)
            {
                case FileSystemSource.Directory:
                    return GetDirectoryName(change.FileOrDirectoryPath);

                case FileSystemSource.File:
                    return Path.GetFileName(change.FileOrDirectoryPath);

                default:
                    throw new InvalidOperationException();
            }
        }

        private static string GetDirectoryName(string path)
        {
            return path.Contains(Path.DirectorySeparatorChar)
                       ? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                       : path;
        }
    }
}