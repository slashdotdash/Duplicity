using System;
using System.Threading;
using System.Threading.Tasks;
using Duplicity.DuplicationStrategy;

namespace Duplicity
{
    public sealed class FileSystemChangeConsumer : IConsumeFileSystemChanges
    {
        private readonly IDuplicationHandlerFactory _handlerFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private int _state;

        public FileSystemChangeConsumer(IDuplicationHandlerFactory handlerFactory)
        {            
            _handlerFactory = handlerFactory;                       
        }

        /// <summary>
        /// Consume the file system changes in the queue if not already consuming.
        /// </summary>
        public void Consume(IProduceFileSystemChanges producer)
        {
            UnlessConsuming(() =>
            {
                _cancellationTokenSource = new CancellationTokenSource();

                Task.Factory.StartNew(() => StartConsuming(producer), _cancellationTokenSource.Token);
            });
        }

        private void UnlessConsuming(Action consume)
        {
            if (Interlocked.CompareExchange(ref _state, 1, 0) == 1)
            {
                consume();
            }
        }

        private void StartConsuming(IProduceFileSystemChanges producer)
        {
            FileSystemChange change;

            while (producer.TryTake(out change))
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