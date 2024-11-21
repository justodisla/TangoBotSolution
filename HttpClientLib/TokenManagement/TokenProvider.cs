using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Streaming;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;

namespace HttpClientLib.TokenManagement
{
    /// <summary>
    /// Provides a valid session token by handling authentication and validation.
    /// </summary>
    public class TokenProvider : ITokenProvider
    {
        //private const string SESSION_TOKEN = "api_session_token";
        private const string LoginUrl = "https://api.cert.tastyworks.com/sessions"; //TODO: Build login url from configuration
        private const string TestApiUrl = "https://api.cert.tastyworks.com/accounts/5WU34986/trading-status"; //TODO: Build test api url from configuration 
        
        private readonly HttpClient _httpClient;
        private readonly TokenParser _tokenParser;
        private string? _sessionToken;
        private string? _streamingTokenEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenProvider"/> class.
        /// </summary>
        public TokenProvider()
        {
            _httpClient = TangoBotServiceProvider.GetService<HttpClient>();
            
            _tokenParser = new TokenParser();
            _streamingTokenEndpoint = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(Constants.ACTIVE_API_URL) + 
                TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(Constants.STREAMING_AUTH_TOKEN_ENDPOINT);

             }

        /// <summary>
        /// Retrieves a valid session token, either from storage or by authenticating.
        /// </summary>
        /// <returns>The valid session token if successful; otherwise, null.</returns>
        public async Task<string?> GetValidTokenAsync()
        {
            _sessionToken = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(Constants.VALID_AUTH_TOKEN);

            if (!string.IsNullOrEmpty(_sessionToken) && await IsTokenValidAsync())
            {
                Console.WriteLine("[Info] Using existing valid token.");
                return _sessionToken;
            }

            Console.WriteLine("[Info] No valid token found. Requesting a new token.");

            if (await AuthenticateAsync())
            {
                TangoBotServiceProvider.GetService<IConfigurationProvider>().SetConfigurationValue(Constants.VALID_AUTH_TOKEN, _sessionToken);
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
            IConfigurationProvider? configurationProvider = TangoBotServiceProvider.GetService<IConfigurationProvider>();

            if (configurationProvider == null) 
            {
                //TODO: Log error
                throw new Exception("Unable to access Configuration Provider");
            }

            //TODO: Add user agent as configuration
            var credentials = new Dictionary<string, object>
            {
                { "login", configurationProvider.GetConfigurationValue(Constants.ACTIVE_USER) },
                { "password", configurationProvider.GetConfigurationValue(Constants.ACTIVE_PASSWORD) },
                { "remember-me", true }
            };

            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine("[Debug] Sending authentication request to Tastytrade.");

                var request = new HttpRequestMessage(HttpMethod.Post, LoginUrl)
                {
                    Content = content
                };
                request.Headers.Add("User-Agent", "tangobot/1.0");//TODO: Add user agent as configuration

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("[Debug] Authentication response received from Tastytrade.");
                    Console.WriteLine($"[Debug] Response body: {responseBody}");

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

        #region Streaming token

        /// <summary>
        /// Gets a valid streaming token asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the valid streaming token.</returns>
        public async Task<string> GetValidStreamingToken()
        {
            string sessionToken = await GetValidTokenAsync();

            string streamingToken = TangoBotServiceProvider.GetService<IConfigurationProvider>().GetConfigurationValue(Constants.STREAMING_AUTH_TOKEN);

            if (string.IsNullOrEmpty(streamingToken) || !IsStreamingTokenValid(streamingToken))
            {
                streamingToken = await GetStreamingTokenAsync(sessionToken);
                TangoBotServiceProvider.GetService<IConfigurationProvider>().SetConfigurationValue(Constants.STREAMING_AUTH_TOKEN, streamingToken);

                IsStreamingTokenValid(streamingToken);

            }

            return streamingToken;
        }

        /// <summary>
        /// Checks if the streaming token is valid.
        /// </summary>
        /// <param name="streamingToken">The streaming token to validate.</param>
        /// <returns>True if the streaming token is valid; otherwise, false.</returns>
        private bool IsStreamingTokenValid(string streamingToken)
        {
            var ss = TangoBotServiceProvider.GetService<IStreamService<QuoteDataHistory>>();

            

           // bool isValid = ss.IsStreamingAuthTokenValid().Result;

            return false;
            
        }
        


        /// <summary>
        /// Retrieves a new streaming token asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token to use for authentication.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the new streaming token.</returns>
        private async Task<string?> GetStreamingTokenAsync(string? sessionToken)
        {
            if (string.IsNullOrEmpty(sessionToken))
            {
                throw new ArgumentNullException(nameof(sessionToken), "Session token cannot be null or empty.");
            }

            try
            {   
                var request = new HttpRequestMessage(HttpMethod.Get, _streamingTokenEndpoint);
                request.Headers.Add("Authorization", sessionToken);

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                
                var responseJson = JsonSerializer.Deserialize<StreamingTokenResponse>(responseBody);

                if (responseJson?.Data?.Token != null)
                {
                    return responseJson.Data.Token;
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

            [JsonPropertyName("dxlink-url")]
            public string? DxlinkUrl { get; set; }

            [JsonPropertyName("level")]
            public string? Level { get; set; }
        }

        #endregion

        private class SessionData
        {
            [JsonPropertyName("session_token")]
            public string session_token { get; set; }

            [JsonPropertyName("remember_token")]
            public string remember_token { get; set; }
        }
    }
}
