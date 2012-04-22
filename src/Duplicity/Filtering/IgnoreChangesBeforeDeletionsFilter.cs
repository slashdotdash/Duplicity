using System;
using System.Reactive.Linq;

namespace Duplicity.Filtering
{
    /// <summary>
    /// Filter out any file or directory changes that occur before the file or directory (or one of its ancestors) are deleted.
    /// </summary>
    internal sealed class IgnoreChangesBeforeDeletionsFilter : IObservable<FileSystemChange>
    {
        private readonly IObservable<FileSystemChange> _observable;

        public IgnoreChangesBeforeDeletionsFilter(IObservable<FileSystemChange> observable)
        {
            _observable = observable;
        }

        public IDisposable Subscribe(IObserver<FileSystemChange> observer)
        {
            return Observable.Create<FileSystemChange>(o => _observable.Subscribe(o))
                //.Buffer(TimeSpan.FromSeconds(1))                
                //.Select(changes => changes.GroupBy() )
                //.GroupByUntil(change => change.Change, change => change, observable => observable)
                .Subscribe(observer);
        }
    }
}