using System.IO;

namespace Duplicity.DuplicationStrategy
{
    internal sealed class DirectoryCreated : IDuplicationHandler
    {
        private readonly string _sourceDirectory;
        private readonly string _targetDirectory;

        public DirectoryCreated(string sourceDirectory, string targetDirectory)
        {
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
        }

        public void Handle(string createdDirectory)
        {
            Directory.CreateDirectory(Path.Combine(_targetDirectory, createdDirectory));            
        }
    }
}