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
        //private readonly IHttpClient _httpClient;
        private readonly HttpClient _httpClient;

        public HttpClientImpl()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, HttpContent? content = null)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };
            return await _httpClient.SendAsync(request);
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}
