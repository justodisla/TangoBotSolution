using System.Net.Http;
using System.Threading.Tasks;

namespace TangoBotApi.Http
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await _httpClient.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return await _httpClient.PutAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await _httpClient.DeleteAsync(requestUri);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(method, requestUri) { Content = content };
            return await _httpClient.SendAsync(request);
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }
    }
}
