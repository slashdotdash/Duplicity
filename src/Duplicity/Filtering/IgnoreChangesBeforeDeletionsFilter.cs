using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Duplicity.Filtering
{
    /// <summary>
    /// Filter out any file or directory changes that occur before the file or directory (or one of its ancestors) are deleted.
    /// </summary>
    internal sealed class IgnoreChangesBeforeDeletionsFilter : IObservable<IList<FileSystemChange>>
    {
        private readonly IObservable<IList<FileSystemChange>> _observable;

        public IgnoreChangesBeforeDeletionsFilter(IObservable<IList<FileSystemChange>> observable)
        {
            _observable = observable;
        }
   
        public IDisposable Subscribe(IObserver<IList<FileSystemChange>> observer)
        {
            return _observable.Select(PrioritizeChanges)
                .Subscribe(observer);
        }

        private IList<FileSystemChange> PrioritizeChanges(IList<FileSystemChange> source)
        {
            if (source.Count == 1) return source;

            return source;
        }
    }
}