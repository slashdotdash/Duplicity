using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Duplicity.Filtering.IgnoredFiles
{
    internal sealed class FileMatcher : IMatcher
    {
        private readonly Regex _pattern;

        public FileMatcher(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentNullException("pattern");

            if (pattern.StartsWith("!"))
                throw new ArgumentException("Negated patterns should not be used", "pattern");

            if (pattern.Contains(Path.DirectorySeparatorChar))
                throw new ArgumentException("Matches files only, not directories", "pattern");
    
            Directory.EnumerateFiles()
            _pattern = Regex.Escape(pattern);
        }

        public bool IsMatch(string path)
        {
            throw new NotImplementedException();
        }
    }
}