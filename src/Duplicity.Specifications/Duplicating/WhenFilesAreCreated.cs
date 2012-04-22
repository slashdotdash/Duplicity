using System.IO;
using Machine.Specifications;

namespace Duplicity.Specifications.Duplicating
{
    [Subject(typeof(Duplicator))]
    public sealed class WhenFilesAreCreated : WithADuplicator
    {
        private Establish context = () => CreateDuplicator();
        private Because of = () => File.Create(Path.Combine(SourceDirectory, "New File.txt")).Close();

        private It should_create_new_file = () => Wait.Until(() => File.Exists(Path.Combine(TargetDirectory, "New File.txt")));
    }
}