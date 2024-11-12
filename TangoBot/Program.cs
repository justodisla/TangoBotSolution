using System;
using System.Threading.Tasks;
using TangoBot.HttpClientLib;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting TangoBot...");

            // Initialize the API client
            var apiClient = new TastyTradeApiClient();

            // Sandbox credentials
            var username = "sandboxuser";  // Replace with your sandbox username
            var password = "TTTangoBotSandBoxPass";  // Replace with your sandbox password

            // Authenticate
            bool isAuthenticated = await apiClient.AuthenticateAsync(username, password);

            if (isAuthenticated)
            {
                Console.WriteLine("TangoBot is authenticated and ready to proceed.");
            }
            else
            {
                Console.WriteLine("Authentication failed. Please check your credentials.");
            }
        }
    }
}
