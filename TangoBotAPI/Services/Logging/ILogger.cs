using System;
using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.Logging
{
    public interface ILogger : IInfrService
    {
        void Log(LogLevel logLevel, string source, string message);
        void LogError(string source, string message);
        void LogWarning(string source, string message);
        void LogInformation(string source, string message);
        void LogDebug(string source, string message);
        void LogTrace(string source, string message);
        void SetLogOutputPreferences(LogOutputPreferences preferences);
    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }

    public class LogOutputPreferences
    {
        public bool LogToConsole { get; set; }
        public bool LogToFile { get; set; }
        public bool LogToEventLog { get; set; }
    }
}
