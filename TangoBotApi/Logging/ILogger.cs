using System;

namespace TangoBotApi.Logging
{
    /// <summary>
    /// Defines methods for logging messages with various severity levels.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message with the Trace severity level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogTrace(string message);

        /// <summary>
        /// Logs a message with the Debug severity level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs a message with the Information severity level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs a message with the Warning severity level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs a message with the Error severity level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogError(string message);

        /// <summary>
        /// Logs a message with the Critical severity level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogCritical(string message);

        /// <summary>
        /// Logs a message with the specified severity level.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The message to log.</param>
        void Log(LogLevel level, string message);

        /// <summary>
        /// Logs a message with the specified severity level and exception.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Log(LogLevel level, string message, Exception exception);
    }

    /// <summary>
    /// Specifies the severity level of a log message.
    /// </summary>
    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }
}
