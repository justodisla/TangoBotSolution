using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientLib
{
    public class TastyTradeApiClient
    {
        private readonly HttpClient _httpClient;
        private string? _sessionToken;
        private string? _rememberToken;

        public TastyTradeApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.cert.tastyworks.com") };
        }

        public async Task<bool> AuthenticateAsync(string username, string password, bool rememberMe = true)
        {
            var credentials = new
            {
                login = username,
                password,
                remember_me = rememberMe
            };
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("/sessions", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Authentication failed with status code {response.StatusCode}.");
                    Console.WriteLine($"Error content: {errorContent}");
                    return false;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var responseJson = JsonSerializer.Deserialize<SessionResponse>(responseBody);

                // Retrieve the session and remember tokens
                _sessionToken = responseJson?.data?.session_token;
                _rememberToken = responseJson?.data?.remember_token;

                if (!string.IsNullOrEmpty(_sessionToken))
                {
                    // Set the session token in the Authorization header
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_sessionToken}");
                    Console.WriteLine("Authentication successful.");
                    Console.WriteLine($"Session Token: {_sessionToken}");
                    Console.WriteLine($"Remember Token: {_rememberToken}");  // Store this securely if needed
                    return true;
                }

                Console.WriteLine("Failed to retrieve session token.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                return false;
            }
        }

        public class SessionResponse
        {
            public SessionData data { get; set; }
        }

        public class SessionData
        {
            public string session_token { get; set; }
            public string remember_token { get; set; }
            public string session_expiration { get; set; }
        }
    }
}
