using Duplicity.DuplicationStrategy;

namespace Duplicity.Specifications.Duplicating.Handlers
{
    public sealed class TestHandler : IDuplicationHandler
    {        
        public bool Handled { get; private set;}
        public FileSystemChange Change { get; private set; }

        public TestHandler(FileSystemChange modification)
        {
            Change = modification;
        } 

        public void Handle(string fileOrDirectory)
        {
            Handled = true;
        }
    }
}
