using HttpClientLib.TokenProviding;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TangoBot.HttpClientLib
{
    public abstract class BaseApiComponent
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        protected BaseApiComponent(HttpClient httpClient, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Sends an authorized GET request to the specified URL.
        /// </summary>
        protected async Task<HttpResponseMessage> SendGetRequestAsync(string url)
        {
            string token = await _tokenProvider.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[Error] Failed to obtain a valid token for API request.");
                return null;
            }

            try
            {
                Console.WriteLine($"[Info] Sending GET request to {url}");

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);

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
    }
}
