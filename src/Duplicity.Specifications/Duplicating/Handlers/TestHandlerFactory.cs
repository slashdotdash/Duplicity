using System.Collections.Generic;
using System.Linq;
using Duplicity.DuplicationStrategy;

namespace Duplicity.Specifications.Duplicating.Handlers
{
    public sealed class TestHandlerFactory : IDuplicationHandlerFactory
    {
        private readonly IList<TestHandler> _handlers = new List<TestHandler>();

        public IEnumerable<TestHandler> Executed
        {
            get { return _handlers.Where(h => h.Handled); }
        }

        public IDuplicationHandler Create(FileSystemChange modification)
        {
            var handler = new TestHandler(modification);

            _handlers.Add(handler);

            return handler;
        }
    }
}