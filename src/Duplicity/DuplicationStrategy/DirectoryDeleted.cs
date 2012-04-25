using System.IO;

namespace Duplicity.DuplicationStrategy
{
    internal sealed class DirectoryDeleted : IDuplicationHandler
    {
        private readonly string _targetDirectory;

        public DirectoryDeleted(string targetDirectory)
        {
            _targetDirectory = targetDirectory;
        }

        public void Handle(string deletedDirectory)
        {
            Directory.Delete(Path.Combine(_targetDirectory, deletedDirectory), true);            
        }
    }
}