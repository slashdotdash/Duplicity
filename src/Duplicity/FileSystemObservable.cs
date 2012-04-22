using System;
using System.Reactive.Subjects;
using System.IO;
using FSWatcher;

namespace Duplicity
{
    /// <summary>
    /// Observe changes to files and directories within the given directory
    /// </summary>
    public sealed class FileSystemObservable : IObservable<FileSystemChange>, IDisposable
    {
        private readonly string _sourceDirectory;
        private readonly Watcher _fileSystemWatcher;
        private readonly Subject<FileSystemChange> _observable = new Subject<FileSystemChange>();

        private bool _startedObserving;

        public FileSystemObservable(string sourceDirectory)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory)) throw new ArgumentNullException("sourceDirectory");
            if (!Directory.Exists(sourceDirectory)) throw new DirectoryNotFoundException("sourceDirectory");

            _sourceDirectory = sourceDirectory;
            
            _fileSystemWatcher = new Watcher(sourceDirectory,
                path => OnDirectoryChange(WatcherChangeTypes.Created, path),
                path => OnDirectoryChange(WatcherChangeTypes.Deleted, path),
                path => OnFileChange(WatcherChangeTypes.Created, path),
                path => OnFileChange(WatcherChangeTypes.Changed, path),
                path => OnFileChange(WatcherChangeTypes.Deleted, path));
        }

        public IDisposable Subscribe(IObserver<FileSystemChange> observer)
        {
            if (!_startedObserving)
            {
                _startedObserving = true;
                _fileSystemWatcher.Watch();
            }

            return _observable.Subscribe(observer);
        }

        public void SetPollFrequency(int milliseconds)
        {
            _fileSystemWatcher.Settings.SetPollFrequencyTo(milliseconds);
        }

        public void Dispose()
        {
            if (_startedObserving)
                _fileSystemWatcher.Dispose();

            _observable.Dispose();
        }        

        private void OnDirectoryChange(WatcherChangeTypes type, string path)
        {
            _observable.OnNext(new FileSystemChange(FileSystemSource.Directory, type, StripSourceDirectory(path)));
        }

        private void OnFileChange(WatcherChangeTypes type, string path)
        {
            _observable.OnNext(new FileSystemChange(FileSystemSource.File, type, StripSourceDirectory(path)));
        }

        private string StripSourceDirectory(string path)
        {
            return path.Remove(0, _sourceDirectory.Length + 1);
        }
    }
}