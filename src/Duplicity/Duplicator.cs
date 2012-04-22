using System;
using System.IO;
using Duplicity.DuplicationStrategy;

namespace Duplicity
{
    /// <summary>
    /// Duplicates file system changes in one directory to another.
    /// </summary>
    /// <remarks>Assumes that targetDirectory contains the same files/folders as the observable source.</remarks>
    public sealed class Duplicator : IDisposable
    {
        private readonly string _sourceDirectory;
        private readonly FileSystemObservable _observable;
        private readonly IDisposable _subscription;
        private readonly DuplicationHandlerFactory _handlerFactory;

        public Duplicator(string sourceDirectory, string targetDirectory)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory)) throw new ArgumentNullException("sourceDirectory");
            if (string.IsNullOrWhiteSpace(targetDirectory)) throw new ArgumentNullException("targetDirectory");
            if (!Directory.Exists(sourceDirectory)) throw new DirectoryNotFoundException();
            if (!Directory.Exists(targetDirectory)) throw new DirectoryNotFoundException();

            _sourceDirectory = sourceDirectory;
            _handlerFactory = new DuplicationHandlerFactory(sourceDirectory, targetDirectory);
            _observable = new FileSystemObservable(sourceDirectory);
            _subscription = _observable.Subscribe(OnFileSystemChange);
        }

        private void OnFileSystemChange(FileSystemChange change)
        {
            var handler = _handlerFactory.Create(change);
            var changedFileOrDirectory = change.FileOrDirectoryName.Remove(0, _sourceDirectory.Length + 1);

            handler.Handle(changedFileOrDirectory);
        }

        public void Dispose()
        {
            _subscription.Dispose();
            _observable.Dispose();
        }
    }
}