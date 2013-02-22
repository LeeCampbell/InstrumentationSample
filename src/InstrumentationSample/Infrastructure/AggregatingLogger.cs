using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace InstrumentationSample.Infrastructure
{
    //A decent IoC container will notice the IDisposable implementation, however I should really provide a hook for the user to be able to dispose too.

    //I use Rx to provide me with a non blocking implementation that is also thread safe(synchronized). Rx would also allow it to be very simple to modify the aggregation strategy e.g. to a sliding window perhaps.
    public sealed class AggregatingLogger : ILogger, IDisposable
    {
        private readonly ILogger _underlyingLogger;
        private readonly Subject<ITrace> _traceEvents = new Subject<ITrace>();
        private readonly IDisposable _traceSubscription;
        private readonly IScheduler _workerScheduler;

        public AggregatingLogger(ILogger underlyingLogger, ISchedulerProvider schedulerProvider)
        {
            _underlyingLogger = underlyingLogger;
            _workerScheduler = schedulerProvider.CreateEventLoopScheduler("AggregatingLogger");
            _traceSubscription = _traceEvents
                //Aggregate and Log asynchronously. Could alternatively use TaskPool or ThreadPool. Needs requirements and use-cases *Discussion point.
                .ObserveOn(_workerScheduler)
                //Make thread safe
                .Synchronize()
                //Split incoming traces into independent sequences based on the trace name (so I don't aggregate Auth calls with WebRequest calls for example)
                .GroupBy(trace => trace.Name)
                //Aggregate on the fly
                .SelectMany(grp => grp.Scan(new TraceAggregator(), (agg, trace) => agg.Add(trace.Elapsed)))
                //Log aggregate values as they happen
                .Subscribe(aggregate => this.Debug("Call Count :'{0}'. Mean average execution time: '{1}'", aggregate.Count, aggregate.MeanAverage()));
        }

        public void Write(LogLevel level, string message, Exception exception)
        {
            _underlyingLogger.Write(level, message, exception);
        }

        public void Instrument(ITrace trace)
        {
            _underlyingLogger.Instrument(trace);
            _traceEvents.OnNext(trace);
        }

        public ITrace StartTrace(string traceName)
        {
            return new Trace(traceName, this);
        }

        public void Dispose()
        {
            _traceSubscription.Dispose();
            //var elsThread = _workerScheduler as IDisposable;
            //if (elsThread != null) elsThread.Dispose();
        }

        //This should be faster than adding to a list and constantly re-summing it for the average. 
        //I also think this is more descriptive than using an anonymous type in this case.
        private sealed class TraceAggregator
        {
            private long _sum;

            public int Count { get; private set; }

            public TraceAggregator Add(TimeSpan elapsed)
            {
                Count = Count + 1;
                _sum += elapsed.Ticks;
                return this;
            }

            public TimeSpan MeanAverage()
            {
                var avgTicks = (_sum / Count);
                return TimeSpan.FromTicks(avgTicks);
            }
        }
    }
}