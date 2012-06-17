using System.IO;
using Duplicity.IO.Async;

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
            var copier = new CopyFileAsync(Path.Combine(_sourceDirectory, createdFile), Path.Combine(_targetDirectory, createdFile), false);
            copier.Execute().Wait();
        }
    }
}