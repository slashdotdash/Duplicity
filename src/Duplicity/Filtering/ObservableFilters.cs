using System;
using System.Collections.Generic;
using Duplicity.Filtering.Aggregation;

namespace Duplicity.Filtering
{
    internal static class ObservableFilters
    {
        /// <summary>
        /// Merge changes in the given observable sequence of buffers to minimise output.
        /// For example, created then deleted changes to the same file will be excluded entirely since the end result is no file.
        /// </summary>
        /// <param name="source">Source sequence to filter.</param>
        /// <returns>An observable sequence of buffers.</returns>
        public static IObservable<IList<FileSystemChange>> PrioritizeFileSystemChanges(this IObservable<IList<FileSystemChange>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new IgnoreChangesBeforeDeletionsFilter(source);
        }
    }
}
