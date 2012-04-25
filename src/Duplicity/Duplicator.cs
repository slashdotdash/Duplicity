﻿using System;
using System.IO;
using System.Reactive.Linq;
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
            if (sourceDirectory == targetDirectory) throw new ArgumentException("Cannot duplicate the source diretory to itself");

            _sourceDirectory = sourceDirectory;
            _handlerFactory = new DuplicationHandlerFactory(sourceDirectory, targetDirectory);
            _observable = new FileSystemObservable(sourceDirectory);

            _subscription = Observable.Create<FileSystemChange>(observer => _observable.Subscribe(observer))
                // http://stackoverflow.com/questions/9985125/in-rx-how-to-group-latest-items-after-a-period-of-time
                //.Buffer(() => _observable.Throttle(TimeSpan.FromSeconds(1)).Timeout(TimeSpan.FromMinutes(1)))
                //.Delay(TimeSpan.FromSeconds(1))
                .Subscribe(OnFileSystemChange);
        }

        private void OnFileSystemChange(FileSystemChange change)
        {
            var handler = _handlerFactory.Create(change);
            handler.Handle(change.FileOrDirectoryPath);
        }

        public void Dispose()
        {
            _subscription.Dispose();
            _observable.Dispose();
        }
    }
}