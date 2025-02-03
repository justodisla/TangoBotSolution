using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBotApi.Services.Http;

namespace TangoBot.Infrastructure.HttpImpl
{
    /// <summary>
    /// Implements the <see cref="IHttpClient"/> interface to provide HTTP client functionalities.
    /// </summary>
    internal class HttpClientImpl : IHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientImpl"/> class.
        /// </summary>
        public HttpClientImpl()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Sends a DELETE request to the specified URI.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> DeleteAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Sends a GET request to the specified URI.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);

            //return await SendAsyncWithRetries(request, 3, 1000);
        }

        /// <summary>
        /// Sends a POST request to the specified URI.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Sends a PUT request to the specified URI.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> PutAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Sends an HTTP request to the specified URI.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Returns an array of required configuration keys.
        /// </summary>
        /// <returns>An array of required configuration keys.</returns>
        public string[] Requires()
        {
            return Array.Empty<string>();
        }

        /// <summary>
        /// Sends an HTTP request to the specified URI with retries.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        /// <param name="delayMilliseconds">The delay between retries in milliseconds.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> SendAsyncWithRetries(HttpRequestMessage request, int maxRetries, int delayMilliseconds)
        {
            HttpResponseMessage response = null;
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await Task.Delay(delayMilliseconds);
                }
                else
                {
                    break;
                }
            }

            return response;
        }

        /// <summary>
        /// Sets up the HTTP client with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration dictionary.</param>
        public void Setup(Dictionary<string, object> configuration)
        {
            // Example implementation of setup method
            foreach (var config in configuration)
            {
                // Apply configuration settings to the HttpClient if needed
                Console.WriteLine($"Configuring {config.Key} with value {config.Value}");
            }
        }
    }
}
