using System;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using TangoBot.API.Http;
using TangoBot.Core.Api2.Commons;
using TangoBotApi.Common;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Configuration;
using TangoBotApi.Services.Http;

namespace TangoBot.Core.Domain.Services
{
    public abstract class BaseApiComponent : IObservable<HttpResponseEvent>
    {
        private readonly IHttpClient _httpClient;
        private readonly ITokenProvider _tokenProvider;
        private readonly ObservableHelper<HttpResponseEvent> _observerManager;
        private readonly IConfigurationProvider _configurationProvider;

        protected BaseApiComponent()
        {
            _httpClient = ServiceLocator.GetTransientService<IHttpClient>() ?? throw new Exception("HttpClient is null");

            //_httpClient = TangoBotServiceProviderExp.GetSingletonService<HttpClient>() ?? throw new Exception("HttpClient is null");
            var _tokenProvider = ServiceLocator.GetSingletonService<ITokenProvider>() ?? throw new Exception("TokenProvider is null");
            int hc1 = _tokenProvider.GetHashCode();
            var _tokenProvider2 = ServiceLocator.GetSingletonService<ITokenProvider>() ?? throw new Exception("TokenProvider is null");
            int hc2 = _tokenProvider2.GetHashCode();
            var _tokenProvider3 = ServiceLocator.GetTransientService<ITokenProvider>() ?? throw new Exception("TokenProvider is null");
            int hc3 = _tokenProvider2.GetHashCode();
            var _tokenProvider4 = ServiceLocator.GetTransientService<ITokenProvider>() ?? throw new Exception("TokenProvider is null");
            int hc4 = _tokenProvider2.GetHashCode();

            _configurationProvider = ServiceLocator.GetSingletonService<IConfigurationProvider>() ?? throw new Exception("ConfigurationProvider is null");

            Dictionary<string, object> lconfig = new()
            {
                { "ACTIVE_API_URL", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_API_URL) },
                { "LOGIN_URL", "https://api.cert.tastyworks.com/sessions" },
                { "LOGIN_USER", "tangobot" },
                { "LOGIN_PASSWORD", "HyperBerserker?3000" },
                { "STREAMING_TOKEN_URL", _configurationProvider.GetConfigurationValue(AppConstants.ACTIVE_API_URL) },
                { "STREAMING_AUTH_TOKEN_ENDPOINT", _configurationProvider.GetConfigurationValue(AppConstants.STREAMING_AUTH_TOKEN_ENDPOINT) }
            };

            _tokenProvider.Setup(lconfig);

            _observerManager = new ObservableHelper<HttpResponseEvent>();
        }

        /// <summary>
        /// Sends an authorized request to the specified URL with the provided content and HTTP method.
        /// </summary>
        protected async Task<HttpResponseMessage?> SendRequestAsync(string url, HttpMethod method, HttpContent? content = null)
        {
            HttpResponseEvent? httpResponseEvent;
            HttpRequestMessage? request = null;
            HttpResponseMessage? response = null;

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
                    request = ResolveRequest(url, method, content, token);

                    Console.WriteLine("[Info] Sending request...");
                    response = await _httpClient.SendAsync(request);
                    Console.WriteLine("[Info] Request sent. Awaiting response...");

                    //Capture the response into the HttpResponseEvent
                    httpResponseEvent = new HttpResponseEvent(request, response, null);
                    _observerManager.Notify(httpResponseEvent);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine($"[Warning] Unsuccessful response. Status code: {response.StatusCode}");
                        //var _diagnoseComponent = new DiagnoseComponent();
                        //await DiagnoseComponent.DiagnoseAsync(response);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("[Warning] Unauthorized request. Retrying with new token.");
                        token = await _tokenProvider.GetValidTokenAsync();
                        if (!string.IsNullOrEmpty(token))
                        {
                            request = new HttpRequestMessage(method, url)
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

        public IDisposable Subscribe(IObserver<HttpResponseEvent> observer)
        {
            return _observerManager.Subscribe(observer);
        }
    }
}