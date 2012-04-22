using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating
{
    [Subject(typeof(Duplicator))]
    public sealed class WhenDirectoriesAreCreated : WithADuplicator
    {
        private Establish context = () => CreateDuplicator();
        private Because of = () => Directory.CreateDirectory(Path.Combine(SourceDirectory, "New Directory"));

        private It should_create_new_file = () => Wait.Until(() => Directory.Exists(Path.Combine(TargetDirectory, "New Directory")));
    }
}