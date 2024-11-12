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
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.cert.tastyworks.com") };

            // Use the provided credentials
            string username = "tangobotsandboxuser";
            string password = "TTTangoBotSandBoxPass";
            var tokenProvider = new TokenProvider(httpClient, username, password);

            // Get a valid session token
            string sessionToken = await tokenProvider.GetValidTokenAsync();

            if (!string.IsNullOrEmpty(sessionToken))
            {
                Console.WriteLine("Successfully obtained a valid session token.");
                // Proceed with additional API calls using this token
            }
            else
            {
                Console.WriteLine("Failed to obtain a valid session token.");
            }
        }
    }
}
