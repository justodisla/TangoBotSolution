using HttpClientLib;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.API.Configuration;
using TangoBot.API.Toolkit;
using TangoBot.DependecyInjection;

public class AccountStatusComponent : BaseApiComponent
{
    private readonly string _baseAccountUrl;
    
    public AccountStatusComponent()
    {
        // Resolve the base URL dynamically from the configuration provider
        var configurationProvider = TangoBotServiceLocator.GetSingletonService<IConfigurationProvider>()
            ?? throw new Exception("ConfigurationProvider is not available.");
        _baseAccountUrl = $"{configurationProvider.GetConfigurationValue(Constants.ACTIVE_API_URL)}/accounts"; 
    }

    public async Task<TradingStatus?> GetTradingStatusAsync(string accountNumber)
    {
        string url = $"{_baseAccountUrl}/{accountNumber}/trading-status";
        var response = await SendRequestAsync(url, HttpMethod.Get);

        if (response != null && response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var tradingStatus = JsonSerializer.Deserialize<TradingStatus>(responseBody);
            return tradingStatus;
        }
        else
        {
            Console.WriteLine($"[Error] Failed to retrieve trading status. Status code: {response?.StatusCode}");
            return null;
        }
    }
}
