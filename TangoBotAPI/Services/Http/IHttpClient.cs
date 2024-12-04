using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.Http
{
    public interface IHttpClient : IInfrService
    {

        Task<HttpResponseMessage> GetAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> PostAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> PutAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> DeleteAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }

    /*
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
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
            var request = new HttpRequestMessage(method, url) { Content = content };
            return await _httpClient.SendAsync(request);
        }

        public string[] Requires()
        {
            return Array.Empty<string>();
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
*/

}
