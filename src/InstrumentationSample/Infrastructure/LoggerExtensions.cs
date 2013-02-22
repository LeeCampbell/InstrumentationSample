using System;
using System.Globalization;

namespace InstrumentationSample.Infrastructure
{
    /// <summary>
    /// Extensions methods to the <see cref="ILogger"/> interface.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs a message with an exception as Fatal.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Fatal(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Fatal, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message with an exception as Fatal.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Fatal(null, format, args);
        }

        /// <summary>
        /// Logs a message with an exception as Error.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Error(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Error, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message with an exception as Error.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Error(null, format, args);
        }

        /// <summary>
        /// Logs a message with an exception as Info.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Info(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Info, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message with an exception as Info.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Info(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Info(null, format, args);
        }

        /// <summary>
        /// Logs a message with an exception as Warn.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Warn(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Warn, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message with an exception as Warn.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Warn(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Warn(null, format, args);
        }

        /// <summary>
        /// Logs a message with an exception as Debug.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Debug(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Debug, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message as Debug.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Debug(null, format, args);
        }

        /// <summary>
        /// Logs a message with an exception as Trace, the second lowest level.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Trace(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Trace, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message as Trace, the second lowest level.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Trace(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Trace(null, format, args);
        }

        /// <summary>
        /// Logs a message with an exception as Verbose, the lowest level.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="exception">The related <see cref="Exception"/> for the message</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Verbose(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            var formattedMessage = Format(format, args);
            logger.Write(LogLevel.Verbose, formattedMessage, exception);
        }
        /// <summary>
        /// Logs a message as Verbose, the lowest level.
        /// </summary>
        /// <param name="logger">The instance of a logger to log with</param>
        /// <param name="format">The message as a string format</param>
        /// <param name="args">The arguments for the message</param>
        public static void Verbose(this ILogger logger, string format, params object[] args)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            logger.Verbose(null, format, args);
        }

        private static string Format(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                return format;
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}