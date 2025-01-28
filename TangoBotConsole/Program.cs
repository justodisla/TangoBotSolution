using TangoBot.Infrastructure.DependencyInjection;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Logging;
using TangoBotApi.Services.Configuration;
using TangoBot.Core.Api2;
using TangoBot.Core.Domain.Aggregates;
using TangoBot.App.Services;
using TangoBot.App.App;

public class Program
{
    public static void Main(string[] args)
    {
        Application app = new Application();

        var accountService = app.GetService<AccountCustomerReportingService>();

        var acct = accountService.GetAccount("5WU34986");

        var abdto = accountService.GetAccountBalance("5WU34986");

        var cb = abdto.CashBalance;

        app.Terminate();


        // Initialize the logger through the ServiceLocator
        ITangoBotLogger logger = ServiceLocator.GetSingletonService<ITangoBotLogger>();

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

    private static void RunApplication(ITangoBotLogger logger)
    {
        // Example usage of ServiceLocator
        var configProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>();

        configProvider.SetConfigurationValue("key", "value");
        logger.LogInformation("Program.RunApplication", "Configuration value set.");
        // Use configProvider as needed

        IMarketDataManager md = new LiveMarketDataManager("AAPL", DateTime.Now, DateTime.Now, TimeFrame.Day);

        md.Throttle(0);
    }
}
