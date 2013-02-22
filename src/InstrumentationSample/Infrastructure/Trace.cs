using System;
using System.Diagnostics;

namespace InstrumentationSample.Infrastructure
{
    /// <summary>
    /// Provides the ability to trace the time a scope existed for.
    /// </summary>
    /// <remarks>
    /// Leveraging the c# using statement you can easily time a block of code for logging or instrumentation.
    /// </remarks>
    public sealed class Trace : ITrace
    {
        private readonly string _name;
        private readonly ILogger _logger;
        private readonly string _machineName;
        private readonly Stopwatch _stopwatch;

        public Trace(string name, ILogger logger)
        {
            _name = name;
            _logger = logger;
            _machineName = Environment.MachineName;
            _stopwatch = Stopwatch.StartNew();
        }

        public string Name { get { return _name; } }

        public string MachineName { get { return _machineName; } }

        public TimeSpan Elapsed { get { return _stopwatch.Elapsed; } }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.Instrument(this);
        }
    }
}