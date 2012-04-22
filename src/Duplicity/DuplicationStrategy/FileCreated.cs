using System.IO;

namespace Duplicity.DuplicationStrategy
{
    internal sealed class FileCreated : IDuplicationHandler
    {
        private readonly string _sourceDirectory;
        private readonly string _targetDirectory;

        public FileCreated(string sourceDirectory, string targetDirectory)
        {
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
        }

        public void Handle(string createdFile)
        {
            File.Copy(Path.Combine(_sourceDirectory, createdFile), Path.Combine(_targetDirectory, createdFile), false);
        }
    }
}