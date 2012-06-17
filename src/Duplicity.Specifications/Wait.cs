using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Duplicity.Specifications
{
    /// <summary>
    /// Wait until the given predicate evaluates to true before continuing execution.
    /// Re-evaluates the predicate until it is true or the timeout period elapses when a <see cref="TimeoutException"/> is thrown.
    /// </summary>
    internal static class Wait
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromMilliseconds(100);

        public static void Until(Func<bool> waitPredicate)
        {
            Until(waitPredicate, DefaultTimeout, DefaultRetryInterval);
        }

        public static void Until(Func<bool> waitPredicate, TimeSpan timeout, TimeSpan retryInterval)
        {
            new RetryObservable(waitPredicate, timeout, retryInterval);
        }

        internal sealed class RetryObservable
        {
            private readonly Func<bool> _waitPredicate;
            private readonly IScheduler _scheduler = Scheduler.CurrentThread;

            public RetryObservable(Func<bool> waitPredicate, TimeSpan timeout, TimeSpan retryInterval)
            {
                _waitPredicate = waitPredicate;

                Wait(Observable.Interval(retryInterval, _scheduler)
                    .Timeout(_scheduler.Now.Add(timeout), _scheduler))
                    .StartWith(0)
                    .Subscribe(ticks => { });
            }

            public IObservable<long> Wait(IObservable<long> source)
            {
                return Observable.Create<long>(observer => source.Subscribe(
                    ticks =>
                    {
                        if (_waitPredicate())
                            observer.OnCompleted();
                    },
                    exception =>
                    {
                        observer.OnCompleted();

                        if (exception is TimeoutException)
                            throw new Exception();
                    },
                    observer.OnCompleted));
            }
        }
    }
}