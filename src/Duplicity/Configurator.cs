using System;
using System.Reactive.Linq;
using Duplicity.Filtering;

namespace Duplicity
{
    public interface IConfigurator
    {
        IObservable<FileSystemChange> Configure(IObservable<FileSystemChange> observable);
    }

    public sealed class Configurator : IConfigurator
    {
        public IObservable<FileSystemChange> Configure(IObservable<FileSystemChange> observable)
        {
            // http://stackoverflow.com/questions/9985125/in-rx-how-to-group-latest-items-after-a-period-of-time
            return observable.Buffer(() => observable.Throttle(TimeSpan.FromSeconds(2)).Timeout(TimeSpan.FromMinutes(1)))
                .PrioritizeFileSystemChanges()
                .SelectMany(x => x);
        }
    }
}