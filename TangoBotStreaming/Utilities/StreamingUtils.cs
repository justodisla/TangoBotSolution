using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotStreaming.Utilities
{
    public static class StreamingUtils
    {

        /// <summary>
        /// Sends a message asynchronously over the WebSocket connection.
        /// </summary>
        /// <param name="client">The WebSocket client.</param>
        /// <param name="message">The message to send.</param>
        public static async Task SendMessageAsync(ClientWebSocket client, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            //Console.WriteLine($"[Sent] {message}");
        }
    }
}
