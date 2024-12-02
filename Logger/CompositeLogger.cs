using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TangoBotApi.Logging;

namespace TangoBotApi.Infrastructure
{
    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger()
        {
            if (!File.Exists("log.txt"))
            {
                // Create the log.txt file
                File.Create("log.txt").Close();
            }

            _loggers = new List<ILogger>
            {
                new ConsoleLogger(LogLevel.Information),
                new FileLogger("log.txt", LogLevel.Information),
                new EventLogger(LogLevel.Information)
            };
        }

        public void Log(LogLevel logLevel, string source, string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(logLevel, source, message);
            }
        }

        public void LogError(string source, string message) => Log(LogLevel.Error, source, message);
        public void LogWarning(string source, string message) => Log(LogLevel.Warning, source, message);
        public void LogInformation(string source, string message) => Log(LogLevel.Information, source, message);
        public void LogDebug(string source, string message) => Log(LogLevel.Debug, source, message);
        public void LogTrace(string source, string message) => Log(LogLevel.Trace, source, message);

        public void SetLogOutputPreferences(LogOutputPreferences preferences)
        {
            foreach (var logger in _loggers)
            {
                logger.SetLogOutputPreferences(preferences);
            }
        }
    }

    public class ConsoleLogger : ILogger
    {
        private readonly LogLevel _minLogLevel;
        private bool _enabled;

        public ConsoleLogger(LogLevel minLogLevel = LogLevel.Trace)
        {
            _minLogLevel = minLogLevel;
            _enabled = true;
        }

        public void Log(LogLevel logLevel, string source, string message)
        {
            if (_enabled && logLevel >= _minLogLevel)
            {
                Console.WriteLine($"{DateTime.Now} [{logLevel}] [{source}] {message}");
            }
        }

        public void LogError(string source, string message) => Log(LogLevel.Error, source, message);
        public void LogWarning(string source, string message) => Log(LogLevel.Warning, source, message);
        public void LogInformation(string source, string message) => Log(LogLevel.Information, source, message);
        public void LogDebug(string source, string message) => Log(LogLevel.Debug, source, message);
        public void LogTrace(string source, string message) => Log(LogLevel.Trace, source, message);

        public void SetLogOutputPreferences(LogOutputPreferences preferences)
        {
            _enabled = preferences.LogToConsole;
        }
    }

    public class FileLogger : ILogger
    {
        private readonly LogLevel _minLogLevel;
        private readonly string _filePath;
        private bool _enabled;

        public FileLogger(string filePath, LogLevel minLogLevel = LogLevel.Trace)
        {
            _filePath = filePath;
            _minLogLevel = minLogLevel;
            _enabled = true;
        }

        public void Log(LogLevel logLevel, string source, string message)
        {
            if (_enabled && logLevel >= _minLogLevel)
            {
                File.AppendAllText(_filePath, $"{DateTime.Now} [{logLevel}] [{source}] {message}{Environment.NewLine}");
            }
        }

        public void LogError(string source, string message) => Log(LogLevel.Error, source, message);
        public void LogWarning(string source, string message) => Log(LogLevel.Warning, source, message);
        public void LogInformation(string source, string message) => Log(LogLevel.Information, source, message);
        public void LogDebug(string source, string message) => Log(LogLevel.Debug, source, message);
        public void LogTrace(string source, string message) => Log(LogLevel.Trace, source, message);

        public void SetLogOutputPreferences(LogOutputPreferences preferences)
        {
            _enabled = preferences.LogToFile;
        }
    }

    public class EventLogger : ILogger
    {
        private readonly LogLevel _minLogLevel;
        private bool _enabled;

        public EventLogger(LogLevel minLogLevel = LogLevel.Trace)
        {
            _minLogLevel = minLogLevel;
            _enabled = true;
        }

        public void Log(LogLevel logLevel, string source, string message)
        {
            if (_enabled && logLevel >= _minLogLevel)
            {
                LogToSystemEvents($"{DateTime.Now} [{logLevel}] [{source}] {message}");
            }
        }

        private void LogToSystemEvents(string message)
        {
            using (var eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        public void LogError(string source, string message) => Log(LogLevel.Error, source, message);
        public void LogWarning(string source, string message) => Log(LogLevel.Warning, source, message);
        public void LogInformation(string source, string message) => Log(LogLevel.Information, source, message);
        public void LogDebug(string source, string message) => Log(LogLevel.Debug, source, message);
        public void LogTrace(string source, string message) => Log(LogLevel.Trace, source, message);

        public void SetLogOutputPreferences(LogOutputPreferences preferences)
        {
            _enabled = preferences.LogToEventLog;
        }
    }
}
