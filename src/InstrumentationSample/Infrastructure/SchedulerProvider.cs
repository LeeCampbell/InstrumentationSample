using System.Reactive.Concurrency;
using System.Threading;

namespace InstrumentationSample.Infrastructure
{
    //Example of an ISchedulerProvider
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler CurrentThread
        {
            get { return Scheduler.CurrentThread; }
        }

        public IScheduler NewThread
        {
            get { return Scheduler.NewThread; }
        }

        public IScheduler TaskPool
        {
            get { return Scheduler.TaskPool; }
        }

        //Should really expose that this is IDisposable too. Skipped for sample code.
        public IScheduler CreateEventLoopScheduler(string name, bool isBackground = true)
        {
            return new EventLoopScheduler(ts => new Thread(ts) { IsBackground = isBackground, Name = name });
        }
    }
}