using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    /// <summary>
    /// Provides a valid session token by handling authentication and validation.
    /// </summary>
    public class TokenProvider
    {
        // Endpoint URLs and credentials (constants for this example)
        private const string LoginUrl = "https://reqres.in/api/login";
        private const string TestApiUrl = "https://reqres.in/api/users/2";
        private const string Email = "eve.holt@reqres.in";
        private const string Password = "cityslicka";

        private readonly HttpClient _httpClient;
        private readonly TokenParser _tokenParser;
        private readonly TokenFileHandler _tokenFileHandler;
        private string _sessionToken;

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
        public async Task<string> GetValidTokenAsync()
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
                request.Headers.Add("Authorization", $"Bearer {_sessionToken}");

                var response = await _httpClient.SendAsync(request);
                bool isValid = response.IsSuccessStatusCode;

                if (isValid)
                {
                    Console.WriteLine("[Debug] Token is valid.");
            }
                else
            {
                    Console.WriteLine($"[Debug] Token validation failed with status code: {response.StatusCode}");
        }

                return isValid;
        }
            catch (Exception ex)
        {
                Console.WriteLine($"[Error] Exception during token validation: {ex.Message}");
                return false;
            }
            return null;
        }

        /// <summary>
        /// Authenticates with the API to obtain a new session token.
        /// </summary>
        /// <returns>True if authentication is successful; otherwise, false.</returns>
        private async Task<bool> AuthenticateAsync()
        {
            var credentials = new { email = Email, password = Password };
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            int maxRetries = 3;
            int delay = 2000; // Start with a 2-second delay

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                Console.WriteLine("[Debug] Sending authentication request.");
                var response = await _httpClient.PostAsync(LoginUrl, content);

                if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("[Debug] Authentication response received.");
                    Console.WriteLine($"[Debug] Response body: {responseBody}");

                    // Use TokenParser to extract the token
                    _sessionToken = _tokenParser.ParseToken(responseBody);

                        if (!string.IsNullOrEmpty(_sessionToken))
                        {
                        Console.WriteLine("[Info] Authentication successful. Session token obtained.");
                            return true;
                        }
                        else
                        {
                        Console.WriteLine("[Error] Session token is missing in the response.");
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Error] Authentication failed with status code {response.StatusCode}. Error: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                Console.WriteLine($"[Error] Exception during authentication: {ex.Message}");
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
