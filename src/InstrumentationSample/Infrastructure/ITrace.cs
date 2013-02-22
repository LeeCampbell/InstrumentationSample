using System;

namespace InstrumentationSample.Infrastructure
{
    public interface ITrace : IDisposable
    {
        string Name { get; }

        string MachineName { get; }

        TimeSpan Elapsed { get; }

        //If you want to implement something like Google Dapper then we would want Annotations.
        //void Annotate(string message);
    }
}