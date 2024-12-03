using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Http;
using Xunit;

namespace TangoBotApi.Tests
{
    public class HttpClientTests
    {
        private readonly IHttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public HttpClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new System.Net.Http.HttpClient(_httpMessageHandlerMock.Object);
            _httpClient = ServiceLocator.GetSingletonService<IHttpClient>();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnResponse()
        {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

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
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.Created);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

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
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put && req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

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
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

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
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == method && req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _httpClient.SendAsync(method, url, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
