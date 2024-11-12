using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    public class TokenProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _username;
        private readonly string _password;
        private readonly string _tokenFilePath;
        private string _sessionToken;

        public TokenProvider(HttpClient httpClient, string username, string password, string tokenFilePath = "session_token.txt")
        {
            _httpClient = httpClient;
            _username = username;
            _password = password;
            _tokenFilePath = tokenFilePath;
        }

        // Main method to retrieve a valid token
        public async Task<string> GetValidTokenAsync()
        {
            _sessionToken = await LoadTokenFromFileAsync();

            if (!string.IsNullOrEmpty(_sessionToken) && await IsTokenValidAsync())
            {
                Console.WriteLine("Using existing valid token.");
                return _sessionToken;
            }

            Console.WriteLine("No valid token found. Requesting a new token.");
            if (await AuthenticateAsync())
            {
                await SaveTokenToFileAsync(_sessionToken);
                return _sessionToken;
            }

            Console.WriteLine("Failed to obtain a valid token.");
            return null;
        }

        // Validates the token by making a simple API call to an endpoint
        private async Task<bool> IsTokenValidAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/accounts");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        // Saves the token to a file
        private async Task SaveTokenToFileAsync(string token)
        {
            await File.WriteAllTextAsync(_tokenFilePath, token);
            Console.WriteLine("Token saved to file.");
        }

        // Loads the token from a file if it exists
        private async Task<string> LoadTokenFromFileAsync()
        {
            if (File.Exists(_tokenFilePath))
            {
                return await File.ReadAllTextAsync(_tokenFilePath);
            }
            return null;
        }

        // Authenticates with the Tastytrade API to get a new session token
        private async Task<bool> AuthenticateAsync()
        {
            var credentials = new { login = _username, password = _password, remember_me = true };
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            int maxRetries = 3;
            int delay = 2000; // Start with a 2-second delay

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var response = await _httpClient.PostAsync("/sessions", content);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonSerializer.Deserialize<SessionResponse>(responseBody);
                        _sessionToken = responseJson?.data?.session_token;

                        if (!string.IsNullOrEmpty(_sessionToken))
                        {
                            Console.WriteLine("Authentication successful. Session token obtained.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Session token is missing in the response.");
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Attempt {attempt} failed with status code {response.StatusCode}. Error: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt} encountered an error: {ex.Message}");
                }

                // Wait before retrying, with exponential backoff
                await Task.Delay(delay);
                delay *= 2; // Double the delay with each attempt
            }

            return false;
        }

        // Classes to represent the JSON response structure
        private class SessionResponse
        {
            public SessionData data { get; set; }
        }

        private class SessionData
        {
            public string session_token { get; set; }
            public string remember_token { get; set; }
        }

    }
}
