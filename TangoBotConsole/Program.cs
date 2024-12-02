using TangoBotApi.Logging;
using TangoBotApi.Infrastructure;

public class Program
{
    public static void Main(string[] args)
    {
        // Initialize the logger through the ServiceLocator
        ILogger logger = ServiceLocator.GetSingletonService<ILogger>();

        // Configure log output preferences
        var logOutputPreferences = new LogOutputPreferences
        {
            LogToConsole = true,
            LogToFile = true,
            LogToEventLog = false
        };
        logger.SetLogOutputPreferences(logOutputPreferences);

        // Log some messages
        logger.LogInformation("Program.Main", "Application started.");
        logger.LogWarning("Program.Main", "This is a warning message.");
        logger.LogError("Program.Main", "This is an error message.");

        // Run the application (if needed, you can add more logic here)
        RunApplication(logger);
    }

    private static void RunApplication(ILogger logger)
    {
        // Example usage of ServiceLocator
        var configProvider = ServiceLocator.GetSingletonService<TangoBotApi.Configuration.IConfigurationProvider>();

        configProvider.SetConfigurationValue("key", "value");
        logger.LogInformation("Program.RunApplication", "Configuration value set.");
        // Use configProvider as needed
    }
}
