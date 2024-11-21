using System;
using System.IO;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;
using static HttpClientLib.TastyTradeApiClient;


namespace HttpClientLib.TokenManagement
{
    /// <summary>
    /// Provides a valid session token by handling authentication and validation.
    /// </summary>
    public class TokenProvider : ITokenProvider
    {

        private const string API_QUOTE_TOKEN = "api_quote_token";
        private const string SESSION_TOKEN = "api_session_token";



        // Endpoint URLs and credentials for Tastytrade API
        private const string LoginUrl = "https://api.cert.tastyworks.com/sessions"; // Sandbox login endpoint
        private const string TestApiUrl = "https://api.cert.tastyworks.com/accounts/5WU34986/trading-status"; // Test endpoint to validate token
        private const string Username = "tangobotsandboxuser"; // Replace with your TT sandbox username
        private const string Password = "HyperBerserker?3000";//"TTTangoBotSandBoxPass"; // Replace with your TT sandbox password

        private readonly HttpClient _httpClient;
        private readonly TokenParser _tokenParser;
        //private readonly TokenFileHandler _tokenFileHandler;
        private string? _sessionToken;
        private string? _streamingTokenEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenProvider"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient used for API calls.</param>
        public TokenProvider()
        {
            _httpClient = TangoBotServiceProvider.GetService<HttpClient>();
            _tokenParser = new TokenParser();
            //_tokenFileHandler = new TokenFileHandler();

            _streamingTokenEndpoint = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue("apiQuoteTokenEndpoint");
        }

        /// <summary>
        /// Retrieves a valid session token, either from storage or by authenticating.
        /// </summary>
        /// <returns>The valid session token if successful; otherwise, null.</returns>
        public async Task<string?> GetValidTokenAsync()
        {
            // Attempt to load the token from file
            //_sessionToken = await _tokenFileHandler.LoadTokenFromFileAsync();
            _sessionToken = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(SESSION_TOKEN);

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
                //await _tokenFileHandler.SaveTokenToFileAsync(_sessionToken);
                TangoBotServiceProvider.GetService<IConfigurationProvider>().SetConfigurationValue(SESSION_TOKEN, _sessionToken);
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
                    Console.WriteLine($"[Debug] Response body: {responseBody}");
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

            var credentials = new Dictionary<string, object>
            {
                { "login", Username },
                { "password", Password },
                { "remember-me", true }
            };

            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            //var credentials = new { login = Username, password = Password, remember-me: true };
            //var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine("[Debug] Sending authentication request to Tastytrade.");

                var request = new HttpRequestMessage(HttpMethod.Post, LoginUrl)
                {
                    Content = content
                };
                request.Headers.Add("User-Agent", "tangobot/1.0");

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                //var response = await _httpClient.PostAsync(LoginUrl, content);
                //var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

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
                    //Console.WriteLine($"[Error] Response body: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during authentication with Tastytrade: {ex.Message}");
            }

            return false;
        }

        #region Streaming token

        public async Task<string> GetValidStreamingToken()
        {
            string sessionToken = await GetValidTokenAsync();

            // Load token from persistence
            string streamingToken = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(API_QUOTE_TOKEN);

            if (string.IsNullOrEmpty(streamingToken))
            {
                // Get streaming token
                streamingToken = await GetStreamingTokenAsync(sessionToken);
                TangoBotServiceProvider.GetService<IConfigurationProvider>().SetConfigurationValue(API_QUOTE_TOKEN, streamingToken);
            }

            return streamingToken;

        }

        private async Task<string?> GetStreamingTokenAsync(string? sessionToken)
        {
            if (string.IsNullOrEmpty(sessionToken))
            {
                throw new ArgumentNullException(nameof(sessionToken), "Session token cannot be null or empty.");
            }

            try
            {
                string sburl = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(Constants.SANDBOX_URL);

                var cUrl = sburl + _streamingTokenEndpoint;

                var request = new HttpRequestMessage(HttpMethod.Get, cUrl);
                request.Headers.Add("Authorization", sessionToken);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var responseJson = JsonSerializer.Deserialize<StreamingTokenResponse>(responseBody);

                if (responseJson?.Data?.Token != null)
                {
                    string token = responseJson.Data.Token;
                    return token;
                }
                else
                {
                    Console.WriteLine("[Error] Streaming token is missing in the response.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during streaming token retrieval: {ex.Message}");
                return null;
            }
        }

        private class StreamingTokenResponse
        {
            [JsonPropertyName("data")]
            public StreamingTokenData? Data { get; set; }
            public string? Context { get; set; }
        }

        private class StreamingTokenData
        {
            [JsonPropertyName("token")]

            public string? Token { get; set; }
            public string? DxlinkUrl { get; set; }
            public string? Level { get; set; }
        }

        #endregion

        private class SessionData
        {
            public string session_token { get; set; }
            public string remember_token { get; set; }
        }

    }
}
