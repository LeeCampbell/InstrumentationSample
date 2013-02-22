namespace InstrumentationSample.Infrastructure
{
    public interface ILogFactory
    {
        ILogger CreateLogger();
    }
}