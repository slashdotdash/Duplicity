using System;
using System.Collections.Concurrent;
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
        private readonly FileSystemObservable _observable;
        private readonly DuplicationHandlerFactory _handlerFactory;
        private readonly BlockingCollection<FileSystemChange> _changes;

        public bool IsAlive
        {
            get { return _observable.IsAlive; }
        }

        public Duplicator(string sourceDirectory, string targetDirectory, IConfigurator config)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory)) throw new ArgumentNullException("sourceDirectory");
            if (string.IsNullOrWhiteSpace(targetDirectory)) throw new ArgumentNullException("targetDirectory");
            if (!Directory.Exists(sourceDirectory)) throw new DirectoryNotFoundException();
            if (!Directory.Exists(targetDirectory)) throw new DirectoryNotFoundException();
            if (sourceDirectory == targetDirectory) throw new ArgumentException("Cannot duplicate the source directory to itself");
            
            _observable = new FileSystemObservable(sourceDirectory);
            _changes = new BlockingCollection<FileSystemChange>(new FileSystemChangeQueue());
            _handlerFactory = new DuplicationHandlerFactory(sourceDirectory, targetDirectory);
            
            _changes.AddFromObservable(_observable, true);
        }

        public void Dispose()
        {
            _observable.Dispose();
            _changes.Dispose();
        }

        private void OnFileSystemChange(FileSystemChange change)
        {
            var handler = _handlerFactory.Create(change);
            handler.Handle(change.FileOrDirectoryPath);
        }
    }
}