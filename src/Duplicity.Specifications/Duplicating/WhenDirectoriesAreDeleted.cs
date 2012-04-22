using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating
{
    [Subject(typeof(Duplicator))]
    public sealed class WhenDirectoriesAreDeleted : WithADuplicator
    {
        private Establish context = () =>
        {
            ChangedFileOrDirectoryName = Path.Combine(SourceDirectory, "Deleted Directory");
            Directory.CreateDirectory(ChangedFileOrDirectoryName);
            CreateDuplicator();
        };

        private Because of = () => Directory.Delete(ChangedFileOrDirectoryName);

        private It should_delete_direcrtory = () => Wait.Until(() => Directory.GetDirectories(TargetDirectory).Length == 0);
    }
}