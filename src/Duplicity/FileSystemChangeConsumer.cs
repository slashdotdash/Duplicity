using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Duplicity.DuplicationStrategy;

namespace Duplicity
{
    public sealed class FileSystemChangeConsumer
    {
        private readonly BlockingCollection<FileSystemChange> _changes;
        private readonly IDuplicationHandlerFactory _handlerFactory;

        public FileSystemChangeConsumer(BlockingCollection<FileSystemChange> changes, IDuplicationHandlerFactory handlerFactory)
        {
            _changes = changes;
            _handlerFactory = handlerFactory;

            var cts = new CancellationTokenSource();

            Task.Factory.StartNew(Consume);
        }

        private void Consume()
        {
            foreach (var change in _changes.GetConsumingEnumerable())
            {
                HandlerFor(change).Handle(change.FileOrDirectoryPath);
            }
        }

        private IDuplicationHandler HandlerFor(FileSystemChange change)
        {
            return _handlerFactory.Create(change);
        }
    }
}