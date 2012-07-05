using System.Threading;
using Duplicity.DuplicationStrategy;

namespace Duplicity.Specifications.Duplicating.Handlers
{
    public sealed class BlockingHandler : IDuplicationHandler
    {
        public bool Handled { get; private set; }
        public FileSystemChange Change { get; private set; }

        private readonly ManualResetEvent _reset;

        public BlockingHandler(ManualResetEvent reset, FileSystemChange change)
        {
            _reset = reset;
            Change = change;
        }

        public void Handle(string fileOrDirectory)
        {
            _reset.WaitOne();

            Handled = true;
        }
    }
}