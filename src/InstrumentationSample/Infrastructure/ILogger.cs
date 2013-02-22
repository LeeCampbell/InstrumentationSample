using System;

namespace InstrumentationSample.Infrastructure
{
    public interface ILogger
    {
        /// <summary>
        /// Broadcasts a log entry for any of the configured listener instances to write if appropriate
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An optional Exception instance to be associated with the log. Useful for capturing stack traces.</param>
        void Write(LogLevel level, string message, Exception exception);

        //Hmmm pretty crap name...
        void Instrument(ITrace trace);

        ITrace StartTrace(string traceName);
    }
}