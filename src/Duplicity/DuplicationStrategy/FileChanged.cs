using System.IO;

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
            File.Copy(Path.Combine(_sourceDirectory, modifiedFile), Path.Combine(_targetDirectory, modifiedFile), true);
        }
    }
}