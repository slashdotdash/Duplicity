using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating
{
    [Subject(typeof(Duplicator))]
    public sealed class WhenFilesAreDeleted : WithADuplicator
    {
        private Establish context = () =>
        {
            ChangedFileOrDirectoryName = "Deleted File.txt";
            CreateFile(Path.Combine(SourceDirectory, ChangedFileOrDirectoryName));
            CreateFile(Path.Combine(TargetDirectory, ChangedFileOrDirectoryName));                                            
            CreateDuplicator();
        };
        private Because of = () => File.Delete(Path.Combine(SourceDirectory, ChangedFileOrDirectoryName));

        private It should_delete_file = () => Wait.Until(() => Directory.GetFiles(TargetDirectory).Length == 0);

        private static void CreateFile(string filename)
        {
            using (var fs = File.OpenWrite(filename))
            {
                fs.Flush();
            }
        }
    }
}