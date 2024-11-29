using HttpClientLib.TokenManagement;
using System;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using TangoBot.API.DI;
using TangoBot.API.Http;
using TangoBot.API.Observable;
using TangoBot.API.TokenManagement;
using TangoBot.DependecyInjection;

namespace HttpClientLib
{
    public abstract class BaseApiComponent : IObservable<HttpResponseEvent>, ITTService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenProvider _tokenProvider;
        private readonly ObserverManager<HttpResponseEvent> _observerManager;

        protected BaseApiComponent()
        {
            _httpClient = new HttpClient();

            //_httpClient = TangoBotServiceProviderExp.GetSingletonService<HttpClient>() ?? throw new Exception("HttpClient is null");
            _tokenProvider = TangoBotServiceLocator.GetSingletonService<ITokenProvider>() ?? throw new Exception("TokenProvider is null");
            _observerManager = new ObserverManager<HttpResponseEvent>();
        }

        [Obsolete("This method is deprecated, use SendRequestAsync instead.")]
        /// <summary>
        /// Sends an authorized GET request to the specified URL.
        /// </summary>
        protected async Task<HttpResponseMessage> SendGetRequestAsync(string url)
        {
            string? token = await _tokenProvider.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[Error] Failed to obtain a valid token for API request.");
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            try
            {
                Console.WriteLine($"[Info] Sending GET request to {url}");

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);

                //Perform the request
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("[Warning] Unauthorized request. Retrying with new token.");
                    token = await _tokenProvider.GetValidTokenAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        request = new HttpRequestMessage(HttpMethod.Get, url);
                        request.Headers.Add("Authorization", token);
                        response = await _httpClient.SendAsync(request);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception in API request: {ex.Message}");
                throw;
            }
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
                        var _diagnoseComponent = new DiagnoseComponent();
                        await DiagnoseComponent.DiagnoseAsync(response);
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
