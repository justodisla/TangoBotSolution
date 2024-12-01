using System;
using System.Threading.Tasks;

namespace TangoBotApi.Streaming
{
    /// <summary>
    /// Provides streaming functionalities to clients using WebSockets.
    /// </summary>
    public interface IStreamingService
    {
        /// <summary>
        /// Starts streaming to the specified URL with the provided parameters and callback.
        /// </summary>
        /// <param name="url">The URL to stream to.</param>
        /// <param name="parametersJson">The parameters to send as a JSON body.</param>
        /// <param name="onMessageReceived">The callback to execute each time a message is received.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task StartStreamingAsync(string url, string parametersJson, Action<string> onMessageReceived);

        /// <summary>
        /// Stops the current streaming session.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task StopStreamingAsync();
    }
}

