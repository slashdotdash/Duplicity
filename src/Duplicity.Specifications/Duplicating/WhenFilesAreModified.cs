using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating
{
    [Subject(typeof(Duplicator))]
    public sealed class WhenFilesAreModified : WithADuplicator
    {
        private Establish context = () =>
        {
            ChangedFileOrDirectoryName = "Modified File.txt";
            CreateFile(Path.Combine(SourceDirectory, ChangedFileOrDirectoryName));
            CreateFile(Path.Combine(TargetDirectory, ChangedFileOrDirectoryName));                                            
            CreateDuplicator();
        };
        private Because of = () => File.AppendAllText(Path.Combine(SourceDirectory, ChangedFileOrDirectoryName), "Some appended text");

        private It should_update_file = () => Wait.Until(() => File.ReadAllText(Path.Combine(TargetDirectory, ChangedFileOrDirectoryName)) == "Some appended text");

        private static void CreateFile(string filename)
        {
            using (var fs = File.OpenWrite(filename))
            {
                fs.Flush();
            }
        }
    }
}