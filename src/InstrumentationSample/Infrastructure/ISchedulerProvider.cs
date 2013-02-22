using System.Reactive.Concurrency;

namespace InstrumentationSample.Infrastructure
{
    public interface ISchedulerProvider
    {
        IScheduler CurrentThread { get; }
        
        IScheduler NewThread { get; }

        IScheduler TaskPool { get; }

        IScheduler CreateEventLoopScheduler(string name, bool isBackground = true);
    }
}