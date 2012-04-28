using System.Collections.Generic;
using System.Linq;

namespace Duplicity.Filtering.IgnoredFiles.GitIgnore
{
    internal sealed class GitIgnoreFilter : IFileSystemChangeFilter
    {
        private readonly IList<IMatcher> _included = new List<IMatcher>();
        private readonly IList<IMatcher> _excluded = new List<IMatcher>();

        /// <summary>
        /// Any file system changes matching will be included, taking precedence over any exclusions.
        /// </summary>
        public void Include(IMatcher matcher)
        {
            _included.Add(matcher);
        }

        /// <summary>
        /// Any file system changes matching will be excluded
        /// </summary>
        public void Exclude(IMatcher matcher)
        {
            _excluded.Add(matcher);
        }

        /// <summary>
        /// Should the given change be filtered out given the configured .gitignore rules?
        /// </summary>
        /// <returns>true to exclude/ignore the change, otherwise false.</returns>
        public bool Filter(FileSystemChange change)
        {
            if (_included.Count == 0 && _excluded.Count == 0) return false;

            if (ShouldBeIncluded(change)) return false;

            return ShouldBeExcluded(change);
        }

        private bool ShouldBeIncluded(FileSystemChange change)
        {
            return _included.Any(inclusion => inclusion.IsMatch(change));
        }

        private bool ShouldBeExcluded(FileSystemChange change)
        {
            return _excluded.Any(inclusion => inclusion.IsMatch(change));
        }
    }
}