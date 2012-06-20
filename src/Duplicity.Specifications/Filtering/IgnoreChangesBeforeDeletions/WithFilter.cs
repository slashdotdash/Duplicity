using System;
using System.Collections.Generic;
using System.Linq;
using Duplicity.Filtering.Aggregation;
using Machine.Specifications;

namespace Duplicity.Specifications.Filtering.IgnoreChangesBeforeDeletions
{
    public abstract class WithFilter
    {        
        private static IDisposable _subscription;

        protected static InputBuilder Input;
        protected static List<FileSystemChange> Output;

        protected Establish Context = () =>
                                          {                                              
                                              Input = new InputBuilder();
                                              Output = new List<FileSystemChange>();
                                          };
       
        protected static FileSystemChange OutputAt(int index)
        {
            return Output.ElementAt(index);
        }

        protected static void Filter()
        {
            _subscription = new IgnoreChangesBeforeDeletionsFilter(Input.ToBufferedObservable())
                .Subscribe(change => Output.AddRange(change));
        }

        protected Cleanup Dispose = () => _subscription.Dispose();
    }
}