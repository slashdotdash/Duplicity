using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Duplicity.Filtering.IgnoredFiles.GitIgnore
{
    /// <summary>
    /// Parses either a .gitignore file ar a collection of input strings and creates a file system change filter according to the given pattern(s).
    /// </summary>
    public sealed class GitIgnoreParser
    {
        private readonly IList<string> _input;

        public GitIgnoreParser(string pathToGitIgnoreFile)
        {
            if (!File.Exists(pathToGitIgnoreFile))
                throw new FileNotFoundException(string.Format(@"Could not find .gitignore file: ""{0}""", pathToGitIgnoreFile));

            _input = File.ReadAllLines(pathToGitIgnoreFile);
        }

        public GitIgnoreParser(IEnumerable<string> input)
        {
            _input = input.ToArray();
        }

        public IFileSystemChangeFilter CreateFilter()
        {
            var filter = new GitIgnoreFilter();

            foreach (var line in _input)
            {
                // Ignore blank or comment lines
                if (IsBlankLine(line) || IsCommentedLine(line)) continue;

                if (IsNegatedPattern(line))
                {
                    filter.Include(CreatePatternMatcher(line.Substring(1)));
                }
                else
                {
                    filter.Exclude(CreatePatternMatcher(line));
                }
            }

            return filter;
        }

        /// <summary>
        /// A blank line matches no files, so it can serve as a separator for readability.
        /// </summary>
        private static bool IsBlankLine(string line)
        {
            return string.IsNullOrWhiteSpace(line);
        }

        /// <summary>
        /// A line starting with # serves as a comment.
        /// </summary>
        private static bool IsCommentedLine(string line)
        {
            return line.StartsWith("#");
        }

        /// <summary>
        /// An optional prefix ! which negates the pattern; any matching file excluded by a previous pattern will become included again. 
        /// If a negated pattern matches, this will override lower precedence patterns sources.
        /// </summary>
        private static bool IsNegatedPattern(string line)
        {
            return line.StartsWith("!");
        }

        private static IMatcher CreatePatternMatcher(string pattern)
        {
            pattern = pattern.Trim();

            // If the pattern ends with a slash it will match a directory only
            if (pattern.EndsWith("/"))
                return new DirectoryMatcher(pattern);

            // If the pattern does not contain a slash /, git treats it as a shell glob pattern and checks for a match against the pathname without leading directories.
            if (!pattern.Contains("/"))
                return new FileMatcher(pattern);

            return new PathMatcher(pattern);
        }
    }
}