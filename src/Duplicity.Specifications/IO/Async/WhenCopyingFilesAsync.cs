using System.IO;
using Duplicity.IO.Async;
using Machine.Specifications;

namespace Duplicity.Specifications.IO.Async
{
    [Subject(typeof (CopyFileAsync))]
    public sealed class WhenCopyingFilesAsync
    {
        private static string tmpDirectory, source, destination;
        private static CopyFileAsync copier;

        private Establish context = () =>
                                        {
                                            tmpDirectory = TempPath.GetTempDirectoryName();
                                            source = Path.Combine(tmpDirectory, "Source.txt");
                                            destination = Path.Combine(tmpDirectory, "Copied.txt");

                                            File.WriteAllText(source, "Example content");
                                            
                                            copier = new CopyFileAsync(source, destination, false);
                                        };

        private Because of = () => copier.Execute().Wait();

        private It should_copy_file_asynchronously = () => File.Exists(destination).ShouldBeTrue();
        private It should_copy_file_contents = () => File.ReadAllText(destination).ShouldEqual("Example content");

        private Cleanup Dispose = () => Directory.Delete(tmpDirectory, true);
    }
}