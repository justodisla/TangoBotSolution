namespace TangoBot.API.Logging
{
    public interface ILoggingService
    {
        void LogInformation(Type source, string message, params object[] args);
        void LogWarning(Type source, string message, params object[] args);
        void LogError(Type source, Exception exception, string message, params object[] args);
    }
}
