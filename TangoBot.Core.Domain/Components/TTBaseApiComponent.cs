using System.Text.Json;
using TangoBot.API.Http;
using TangoBot.Core.Api2.Commons;
using TangoBotApi.Common;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;
using TangoBotApi.Services.Http;

namespace TangoBot.Core.Domain.Services
{
    /// <summary>
    /// Provides a base class for API components to interact with the TastyTrade API.
    /// </summary>
    public abstract class TTBaseApiComponent : IObservable<HttpResponseEvent>
    {
        private readonly IHttpClient _httpClient;
        private readonly ITokenProvider _tokenProvider;
        private readonly ObservableHelper<HttpResponseEvent> _observerManager;
        private readonly IConfigurationProvider _configurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TTBaseApiComponent"/> class.
        /// </summary>
        protected TTBaseApiComponent()
        {
            _httpClient = ServiceLocator.GetTransientService<IHttpClient>() ?? throw new Exception("HttpClient is null");
            _tokenProvider = ServiceLocator.GetSingletonService<ITokenProvider>() ?? throw new Exception("TokenProvider is null");
            _configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>() ?? throw new Exception("ConfigurationProvider is null");

            Dictionary<string, object> lconfig = new()
            {
                { "ACTIVE_API_URL", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_API_URL) },
                { "LOGIN_URL", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_API_URL) + "/sessions" },
                { "LOGIN_USER", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_USER) },
                { "LOGIN_PASSWORD", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_PASSWORD) },
                { "STREAMING_TOKEN_URL", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_API_URL) },
                { "STREAMING_AUTH_TOKEN_ENDPOINT", _configurationProvider.GetConfigurationValue(AppConstants.STREAMING_AUTH_TOKEN_ENDPOINT) },
                { "REMEMBER_ME", true }
            };

            _tokenProvider.Setup(lconfig);
            _observerManager = new ObservableHelper<HttpResponseEvent>();
        }

        /// <summary>
        /// Sends an authorized request to the specified URL with the provided content and HTTP method.
        /// </summary>
        /// <param name="endPoint">The endpoint to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="content">The content to include in the request, if any.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        protected async Task<HttpResponseMessage?> SendRequestAsync(string endPoint, HttpMethod method, HttpContent? content = null)
        {
            HttpResponseEvent? httpResponseEvent;
            HttpRequestMessage? request = null;
            HttpResponseMessage? response = null;

            string urlBase = _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_API_URL);
            var uri = string.Concat(urlBase, "/", endPoint);

            string? token = await _tokenProvider.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[Error] Failed to obtain a valid token for API request.");
                return null;
            }

            int maxRetries = 3;
            int delay = 2000; // 2 seconds

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    request = ResolveRequest(uri, method, content, token);

                    Console.WriteLine("[Info] Sending request...");
                    response = await _httpClient.SendAsync(request);
                    Console.WriteLine("[Info] Request sent. Awaiting response...");

                    // Capture the response into the HttpResponseEvent
                    httpResponseEvent = new HttpResponseEvent(request, response, null);
                    _observerManager.Notify(httpResponseEvent);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine($"[Warning] Unsuccessful response. Status code: {response.StatusCode}");
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("[Warning] Unauthorized request. Retrying with new token.");
                        token = await _tokenProvider.GetValidTokenAsync();
                        if (!string.IsNullOrEmpty(token))
                        {
                            request = new HttpRequestMessage(method, uri)
                            {
                                Content = content
                            };
                            request.Headers.Add("Authorization", token);
                            response = await _httpClient.SendAsync(request);
                        }
                    }

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Info] Response received. Status code: {response.StatusCode}, Content: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("[Info] Successful response.");
                        return response;
                    }
                    else
                    {
                        Console.WriteLine($"[Warning] Unsuccessful response. Status code: {response.StatusCode}, Content: {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    httpResponseEvent = new HttpResponseEvent(request, response, ex);
                    _observerManager.Notify(httpResponseEvent);

                    Console.WriteLine($"[Error] Exception in API request: {ex.Message}");
                    if (i == maxRetries - 1)
                    {
                        throw;
                    }
                }

                Console.WriteLine($"[Info] Waiting for {delay}ms before retrying...");
                await Task.Delay(delay);
            }

            return null;
        }

        /// <summary>
        /// Resolves the HTTP request message with the specified URL, method, content, and token.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="content">The content to include in the request, if any.</param>
        /// <param name="token">The authorization token to include in the request headers.</param>
        /// <returns>The resolved HTTP request message.</returns>
        private static HttpRequestMessage ResolveRequest(string url, HttpMethod method, HttpContent? content, string? token)
        {
            Console.WriteLine($"[Info] Sending {method} request to {url}");

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };
            request.Headers.Add("Authorization", token);
            return request;
        }

        /// <summary>
        /// Subscribes an observer to receive notifications of HTTP response events.
        /// </summary>
        /// <param name="observer">The observer to subscribe.</param>
        /// <returns>A disposable object that can be used to unsubscribe the observer.</returns>
        public IDisposable Subscribe(IObserver<HttpResponseEvent> observer)
        {
            return _observerManager.Subscribe(observer);
        }

        /// <summary>
        /// Parses the HTTP response message into a JSON document.
        /// </summary>
        /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
        /// <returns>The parsed JSON document, or null if the response is not successful.</returns>
        internal static JsonDocument? ParseHttpResponseMessage(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var contentStream = httpResponseMessage.Content.ReadAsStream();
            return JsonDocument.Parse(contentStream);
        }

        /// <summary>
        /// Returns an array of strings with services required for this service to be loaded.
        /// </summary>
        /// <returns>An array of required service names.</returns>
        public string[] Requires()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets up the service with the provided configuration.
        /// </summary>
        /// <param name="configuration">A dictionary containing configuration settings.</param>
        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}
