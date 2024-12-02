using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TangoBotApi.Http;
using TangoBotApi.Infrastructure;
using Xunit;

namespace TangoBotApi.Tests
{
    public class HttpClientRealTests
    {
        private readonly IHttpClient _httpClient;

        public HttpClientRealTests()
        {
            var httpClient = new System.Net.Http.HttpClient();
            _httpClient = new HttpClientWrapper(httpClient); // Assuming HttpClientWrapper is your implementation of IHttpClient
        }

        [Fact]
        public async Task GetAsync_ShouldReturnResponse()
        {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnResponse()
        {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts";
            var content = new StringContent("{\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}", System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync(url, content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync_ShouldReturnResponse()
        {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var content = new StringContent("{\"id\":1,\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}", System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PutAsync(url, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResponse()
        {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";

            // Act
            var response = await _httpClient.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_ShouldReturnResponse()
        {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var method = HttpMethod.Patch;
            var content = new StringContent("{\"title\":\"foo\"}", System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.SendAsync(method, url, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
