using System.IO;
using Duplicity.IO.Async;

namespace Duplicity.DuplicationStrategy
{
    internal sealed class FileChanged : IDuplicationHandler
    {
        private readonly string _sourceDirectory;
        private readonly string _targetDirectory;

        public FileChanged(string sourceDirectory, string targetDirectory)
        {
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
        }

        public void Handle(string modifiedFile)
        {
            var copier = new CopyFileAsync(Path.Combine(_sourceDirectory, modifiedFile), Path.Combine(_targetDirectory, modifiedFile), true);
            copier.Execute().Wait();
        }
    }
}