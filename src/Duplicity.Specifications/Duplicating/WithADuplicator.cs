using System.IO;
using System.Threading;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating
{
    public abstract class WithADuplicator
    {
        protected static string SourceDirectory;
        protected static string TargetDirectory;
        protected static string ChangedFileOrDirectoryName;

        private static Duplicator _duplicator;

        protected Establish Context = () =>
        {
            SourceDirectory = TempPath.GetTempDirectoryName();
            TargetDirectory = TempPath.GetTempDirectoryName();
        };

        protected static void CreateDuplicator()
        {
            _duplicator = new Duplicator(SourceDirectory, TargetDirectory);

            Thread.Sleep(1000);
        }

        protected Cleanup Dispose = () =>
        {
            _duplicator.Dispose();

            Directory.Delete(SourceDirectory, true);
            Directory.Delete(TargetDirectory, true);
        };
    }
}