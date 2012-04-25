using System;
using System.Collections.Generic;

namespace Duplicity.Filtering
{
    internal static class ObservableFilters
    {
        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping buffers.        
        /// </summary>
        /// <param name="source">Source sequence to produce buffers over.</param><param name="bufferClosingSelector">A function invoked to define the boundaries of the produced buffers. A new buffer is started when the previous one is closed.</param>
        /// <returns>
        /// An observable sequence of buffers.
        /// </returns>
        public static IObservable<IList<FileSystemChange>> PrioritizeFileSystemChanges(this IObservable<IList<FileSystemChange>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new IgnoreChangesBeforeDeletionsFilter(source);
        }
    }
}
