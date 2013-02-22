using System;
using System.Globalization;
using System.Threading;

namespace InstrumentationSample.Infrastructure
{
    public sealed class ConsoleLogger : ILogger
    {
        private readonly string _loggedTypeName;

        public ConsoleLogger(Type loggedType)
        {
            _loggedTypeName = loggedType.Name;
        }

        public void Write(LogLevel level, string message, Exception exception)
        {
            using (new ConsoleColorScope(LevelToColor(level)))
            {
                var threadName = Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);
                Console.WriteLine("{0:o} {1} [{2}] {3} {4} {5}", DateTimeOffset.UtcNow, level, threadName, _loggedTypeName, message, exception);    
            }
        }

        public ITrace StartTrace(string traceName)
        {
            return new Trace(traceName, this);
        }

        public void Instrument(ITrace trace)
        {
            this.Verbose("{0} took {1}", trace.Name, trace.Elapsed);
        }

        private static ConsoleColor LevelToColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Verbose:
                    return ConsoleColor.DarkGray;
                case LogLevel.Trace:
                    return ConsoleColor.Gray;
                case LogLevel.Debug:
                    return ConsoleColor.White;
                case LogLevel.Warn:
                    return ConsoleColor.Yellow;
                case LogLevel.Info:
                    return ConsoleColor.Green;
                case LogLevel.Error:
                    return ConsoleColor.DarkRed;
                case LogLevel.Fatal:
                    return ConsoleColor.Red;
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        //Little helper class to set the console colour, and revert back once its scope is over.
        //  Allows for nesting of scopes to make console colouring easy.
        sealed class ConsoleColorScope : IDisposable
        {
            private readonly ConsoleColor _previousForegroundColor;

            public ConsoleColorScope(ConsoleColor foregroundColor)
            {
                _previousForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
            }

            public void Dispose()
            {
                Console.ForegroundColor = _previousForegroundColor;
            }
        }
    }
}