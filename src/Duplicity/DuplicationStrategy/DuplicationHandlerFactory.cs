using System;
using System.IO;

namespace Duplicity.DuplicationStrategy
{
    internal class DuplicationHandlerFactory : IDuplicationHandlerFactory
    {
        private readonly IDuplicationHandler _fileCreatedHandler;
        private readonly IDuplicationHandler _fileDeletedHandler;
        private readonly IDuplicationHandler _fileChangedHandler;
        private readonly IDuplicationHandler _directoryCreatedHandler;
        private readonly IDuplicationHandler _directoryDeletedHandler;

        public DuplicationHandlerFactory(string sourceDirectory, string targetDirectory)
        {
            _fileCreatedHandler = new FileCreated(sourceDirectory, targetDirectory);
            _fileDeletedHandler = new FileDeleted(sourceDirectory, targetDirectory);
            _fileChangedHandler = new FileChanged(sourceDirectory, targetDirectory);
            _directoryCreatedHandler = new DirectoryCreated(sourceDirectory, targetDirectory);
            _directoryDeletedHandler = new DirectoryDeleted(targetDirectory);
        }

        public IDuplicationHandler Create(FileSystemChange modification)
        {
            if (IsFileChange(modification)) return FileHandler(modification);
            if (IsDirectoryChange(modification)) return DirectoryHandler(modification);

            throw new NotSupportedException();
        }

        private static bool IsFileChange(FileSystemChange modification)
        {
            return modification.Source == FileSystemSource.File;
        }

        private static bool IsDirectoryChange(FileSystemChange modification)
        {
            return modification.Source == FileSystemSource.Directory;
        }

        private IDuplicationHandler DirectoryHandler(FileSystemChange modification)
        {
            switch (modification.Change)
            {
                case WatcherChangeTypes.Created:
                    return _directoryCreatedHandler;

                case WatcherChangeTypes.Deleted:
                    return _directoryDeletedHandler;
            }

            throw new InvalidOperationException();
        }

        private IDuplicationHandler FileHandler(FileSystemChange modification)
        {
            switch (modification.Change)
            {
                case WatcherChangeTypes.Created:
                    return _fileCreatedHandler;

                case WatcherChangeTypes.Changed:
                    return _fileChangedHandler;

                case WatcherChangeTypes.Deleted:
                    return _fileDeletedHandler;
            }

            throw new InvalidOperationException();
        }
    }
}