using Microsoft.Extensions.Logging;
using TangoBot.API.DI;
using TangoBot.API.Logging;

namespace TangoBot.Infrastructure.Logging
{
    public class LoggingService : ILoggingService, ITTService
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggingService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void LogInformation(Type source, string message, params object[] args)
        {
            var logger = _loggerFactory.CreateLogger(source);
            logger.LogInformation(message, args);
        }

        public void LogWarning(Type source, string message, params object[] args)
        {
            var logger = _loggerFactory.CreateLogger(source);
            logger.LogWarning(message, args);
        }

        public void LogError(Type source, Exception exception, string message, params object[] args)
        {
            var logger = _loggerFactory.CreateLogger(source);
            logger.LogError(exception, message, args);
        }
    }
}
