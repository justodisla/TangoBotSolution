using Microsoft.Extensions.Logging;
using TangoBot.API.Logging;

namespace TangoBot.Infrastructure.Logging
{
    public class LoggingService<T> : ILoggingService<T>
    {
        private readonly ILogger<T> _logger;

        public LoggingService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }
    }
}
