using HttpClientLib.TokenManagement;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientLib
{
    public abstract class BaseApiComponent
    {
        protected readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        protected BaseApiComponent()
        {
            _httpClient = new HttpClient();
            _tokenProvider = new TokenProvider(_httpClient);
        }

        #region Obsolete
        /// <summary>
        /// Sends an authorized GET request to the specified URL.
        /// </summary>
        [Obsolete("SendGetRequestAsync is deprecated. Use SendRequestAsync instead.")]

        protected async Task<HttpResponseMessage?> SendGetRequestAsync(string url)
        {
            string? token = await _tokenProvider.GetValidTokenAsync();
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

        /// <summary>
        /// Sends an authorized POST request to the specified URL with the provided content.
        /// </summary>
        [Obsolete("SendPostRequestAsync is deprecated. Use SendRequestAsync instead.")]
        protected async Task<HttpResponseMessage?> SendPostRequestAsync(string url, HttpContent content)
        {
            string? token = await _tokenProvider.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[Error] Failed to obtain a valid token for API request.");
                return null;
            }

            try
            {
                Console.WriteLine($"[Info] Sending POST request to {url}");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Add("Authorization", token);

                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("[Warning] Unauthorized request. Retrying with new token.");
                    token = await _tokenProvider.GetValidTokenAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, url)
                        {
                            Content = content
                        };
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
        /// Sends an authorized DELETE request to the specified URL.
        /// </summary>
        [Obsolete("SendDeleteRequestAsync is deprecated. Use SendRequestAsync instead.")]
        protected async Task<HttpResponseMessage?> SendDeleteRequestAsync(string url)
        {
            string? token = await _tokenProvider.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[Error] Failed to obtain a valid token for API request.");
                return null;
            }

            try
            {
                Console.WriteLine($"[Info] Sending DELETE request to {url}");

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Add("Authorization", token);

                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("[Warning] Unauthorized request. Retrying with new token.");
                    token = await _tokenProvider.GetValidTokenAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        request = new HttpRequestMessage(HttpMethod.Delete, url);
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

        #endregion
       
        /// <summary>
        /// Sends an authorized request to the specified URL with the provided content and HTTP method.
        /// </summary>
        protected async Task<HttpResponseMessage?> SendRequestAsync(string url, HttpMethod method, HttpContent? content = null)
        {
            string? token = await _tokenProvider.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("[Error] Failed to obtain a valid token for API request.");
                return null;
            }

            try
            {
                HttpRequestMessage request = ResolveToken(url, method, content, token);

                var response = await _httpClient.SendAsync(request);

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

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception in API request: {ex.Message}");
                throw;
            }
        }

        private static HttpRequestMessage ResolveToken(string url, HttpMethod method, HttpContent? content, string? token)
        {
            Console.WriteLine($"[Info] Sending {method} request to {url}");

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };
            request.Headers.Add("Authorization", token);
            return request;
        }
    }
}
