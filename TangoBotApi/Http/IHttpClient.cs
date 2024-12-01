using System.Net.Http;
using System.Threading.Tasks;

namespace TangoBotApi.Http
{
    internal interface IHttpClient
    {
        /// <summary>
        /// Sends an asynchronous GET request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> GetAsync(string url);

        /// <summary>
        /// Sends an asynchronous POST request to the specified URL with the provided content.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="content">The content to send in the POST request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);

        /// <summary>
        /// Sends an asynchronous PUT request to the specified URL with the provided content.
        /// </summary>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="content">The content to send in the PUT request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> PutAsync(string url, HttpContent content);

        /// <summary>
        /// Sends an asynchronous DELETE request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> DeleteAsync(string url);

        /// <summary>
        /// Sends an asynchronous request with the specified HTTP method, URL, and content.
        /// </summary>
        /// <param name="method">The HTTP method to use for the request.</param>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="content">The content to send in the request, if any.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, HttpContent? content = null);
    }
}
