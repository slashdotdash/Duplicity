using System.IO;

namespace Duplicity.DuplicationStrategy
{
    internal sealed class FileDeleted : IDuplicationHandler
    {
        private readonly string _sourceDirectory;
        private readonly string _targetDirectory;

        public FileDeleted(string sourceDirectory, string targetDirectory)
        {
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
        }

        public void Handle(string deletedFile)
        {
            File.Delete(Path.Combine(_targetDirectory, deletedFile));
        }
    }
}