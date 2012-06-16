using System;
using System.IO;
using System.Reactive.Linq;
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
            _duplicator = new Duplicator(SourceDirectory, TargetDirectory, new TestConfiguration());

            Wait.Until(() => _duplicator.IsAlive);
        }

        protected Cleanup Dispose = () =>
        {
            _duplicator.Dispose();

            Directory.Delete(SourceDirectory, true);
            Directory.Delete(TargetDirectory, true);
        };
    }

    internal sealed class TestConfiguration : IConfigurator
    {
        public IObservable<FileSystemChange> Configure(IObservable<FileSystemChange> observable)
        {
            return observable.Do(Console.WriteLine);
        }
    }
}