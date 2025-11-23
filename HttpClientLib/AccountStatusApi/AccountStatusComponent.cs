using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HttpClientLib;

public class AccountStatusComponent : BaseApiComponent
{
    private const string BaseAccountUrl = "https://api.tastyworks.com/accounts";

    public async Task<TradingStatus?> GetTradingStatusAsync(string accountNumber)
    {
        string url = $"{BaseAccountUrl}/{accountNumber}/trading-status";
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
