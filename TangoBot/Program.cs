using System;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBot.HttpClientLib;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();

            var tokenProvider = new TokenProvider(httpClient);

            // Obtain a valid session token
            string sessionToken = await tokenProvider.GetValidTokenAsync();

            if (!string.IsNullOrEmpty(sessionToken))
            {
                Console.WriteLine("[Info] Successfully obtained a valid session token.");

                // Make an authenticated API call using the token
                var request = new HttpRequestMessage(HttpMethod.Get, "https://reqres.in/api/users/2");
                request.Headers.Add("Authorization", $"Bearer {sessionToken}");

                try
                {
                    var response = await httpClient.SendAsync(request);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("[Info] API call response:");
                    Console.WriteLine(responseBody);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Exception during API call: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("[Error] Failed to obtain a valid session token.");
            }
        }
    }
}
