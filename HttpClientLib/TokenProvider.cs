using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static TangoBot.HttpClientLib.TastyTradeApiClient;

namespace TangoBot.HttpClientLib
{
    /// <summary>
    /// Provides a valid session token by handling authentication and validation.
    /// </summary>
    public class TokenProvider
    {
        
        // Endpoint URLs and credentials for Tastytrade API
        private const string LoginUrl = "https://api.cert.tastyworks.com/sessions"; // Sandbox login endpoint
        private const string TestApiUrl = "https://api.cert.tastyworks.com/accounts/5WU34986/trading-status"; // Test endpoint to validate token
        private const string Username = "tangobotsandboxuser"; // Replace with your TT sandbox username
        private const string Password = "TTTangoBotSandBoxPass"; // Replace with your TT sandbox password

        private readonly HttpClient _httpClient;
        private readonly TokenParser _tokenParser;
        private readonly TokenFileHandler _tokenFileHandler;
        private string? _sessionToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenProvider"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient used for API calls.</param>
        public TokenProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _tokenParser = new TokenParser();
            _tokenFileHandler = new TokenFileHandler();
        }

        /// <summary>
        /// Retrieves a valid session token, either from storage or by authenticating.
        /// </summary>
        /// <returns>The valid session token if successful; otherwise, null.</returns>
        public async Task<string?> GetValidTokenAsync()
        {
            // Attempt to load the token from file
            _sessionToken = await _tokenFileHandler.LoadTokenFromFileAsync();

            // Validate the loaded token
            if (!string.IsNullOrEmpty(_sessionToken) && await IsTokenValidAsync())
            {
                Console.WriteLine("[Info] Using existing valid token.");
                return _sessionToken;
            }

            Console.WriteLine("[Info] No valid token found. Requesting a new token.");

            // Authenticate to get a new token
            if (await AuthenticateAsync())
            {
                await _tokenFileHandler.SaveTokenToFileAsync(_sessionToken);
                return _sessionToken;
            }

            Console.WriteLine("[Error] Failed to obtain a valid token.");
            return null;
        }

        /// <summary>
        /// Validates the current session token by making a test API call.
        /// </summary>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        private async Task<bool> IsTokenValidAsync()
        {
            try
            {
                Console.WriteLine("[Debug] Validating the token by making a test API call.");
                var request = new HttpRequestMessage(HttpMethod.Get, TestApiUrl);
                //request.Headers.Add("Authorization", $"Bearer {_sessionToken}");
                request.Headers.Add("Authorization", _sessionToken);

                var response = await _httpClient.SendAsync(request);
                bool isValid = response.IsSuccessStatusCode;

                if (isValid)
                {
                    Console.WriteLine("[Debug] Token is valid.");
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonSerializer.Deserialize<SessionResponse>(responseBody);

                    Console.WriteLine($"[Debug] Token validation failed with status code: {response.StatusCode}");
                }

                return isValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during token validation: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Authenticates with the API to obtain a new session token.
        /// </summary>
        /// <returns>True if authentication is successful; otherwise, false.</returns>
        private async Task<bool> AuthenticateAsync()
        {
            var credentials = new { login = Username, password = Password};
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine("[Debug] Sending authentication request to Tastytrade.");
                var response = await _httpClient.PostAsync(LoginUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("[Debug] Authentication response received from Tastytrade.");
                    Console.WriteLine($"[Debug] Response body: {responseBody}");

                    // Use TokenParser to extract the token
                    _sessionToken = _tokenParser.ParseToken(responseBody);

                    if (!string.IsNullOrEmpty(_sessionToken))
                    {
                        Console.WriteLine("[Info] Authentication successful. Session token obtained from Tastytrade.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("[Error] Session token is missing in the response from Tastytrade.");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Error] Authentication failed with status code {response.StatusCode} from Tastytrade. Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during authentication with Tastytrade: {ex.Message}");
            }

            return false;
        }

        private class SessionData
        {
            public string session_token { get; set; }
            public string remember_token { get; set; }
        }

    }
}
