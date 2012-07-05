using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Duplicity.DuplicationStrategy;

namespace Duplicity.Specifications.Duplicating.Handlers
{
    public sealed class BlockingHandlerFactory : IDuplicationHandlerFactory
    {
        private readonly IList<BlockingHandler> _handlers = new List<BlockingHandler>();

        private readonly ManualResetEvent _reset;

        public IEnumerable<BlockingHandler> Executed
        {
            get { return _handlers.Where(h => h.Handled); }
        }

        public BlockingHandlerFactory(ManualResetEvent reset)
        {
            _reset = reset;
        }

        public IDuplicationHandler Create(FileSystemChange modification)
        {
            var handler = new BlockingHandler(_reset, modification);

            _handlers.Add(handler);

            return handler;
        }
    }
}