using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TangoBotAPI.Streaming;

namespace TangoBotStreaming.Services
{
    public class StreamingService : IStreamService
    {
        private readonly string _webSocketUrl;
        private readonly string _apiQuoteToken;

        public StreamingService(string webSocketUrl, string apiQuoteToken)
        {
            _webSocketUrl = webSocketUrl;
            _apiQuoteToken = apiQuoteToken;
        }

        public async Task StartStreamingAsync()
        {
            using var client = new ClientWebSocket();

            try
            {
                Console.WriteLine("[Info] Connecting to WebSocket...");
                await client.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

                Console.WriteLine("[Info] Connected. Sending SETUP...");
                await SendMessageAsync(client, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

                Console.WriteLine("[Info] Authorizing...");
                await SendMessageAsync(client, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{_apiQuoteToken}\"}}");

                Console.WriteLine("[Info] Opening channel for market data...");
                await SendMessageAsync(client, "{\"type\":\"CHANNEL_REQUEST\",\"channel\":1,\"service\":\"FEED\",\"parameters\":{\"contract\":\"AUTO\"}}");

                Console.WriteLine("[Info] Subscribing to SPY candles...");
                long fromTime = DateTimeOffset.UtcNow.AddDays(-4).ToUnixTimeSeconds();
                await SendMessageAsync(client, $"{{\"type\":\"FEED_SUBSCRIPTION\",\"channel\":1,\"reset\":true,\"add\":[{{\"type\":\"Candle\",\"symbol\":\"SPY{{=1d}}\",\"fromTime\":{fromTime}}}]}}");

                Console.WriteLine("[Info] Waiting for market data...");
                await ReceiveMessagesAsync(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }

        private async Task SendMessageAsync(ClientWebSocket client, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"[Sent] {message}");
        }

        private async Task ReceiveMessagesAsync(ClientWebSocket client)
        {
            var buffer = new byte[8192];

            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                Console.WriteLine($"\n_________________________________________________");
                Console.WriteLine($"\n[Received] {message}");
            }
        }

        public async Task<QuoteDataHistory> StreamHistoricDataAsync(string symbol, DateTime fromTime, DateTime toTime, Timeframe timeframe = Timeframe.Daily, int interval = 1)
        {
            var quoteDataHistory = new QuoteDataHistory();
            using var client = new ClientWebSocket();

            try
            {

                Console.WriteLine("[Info] Connecting to WebSocket...");
                await client.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

                Console.WriteLine("[Info] Connected. Sending SETUP...");
                await SendMessageAsync(client, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":160,\"acceptKeepaliveTimeout\":160}");

                Console.WriteLine("[Info] Authorizing...");
                await SendMessageAsync(client, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{_apiQuoteToken}\"}}");

                Console.WriteLine("[Info] Opening channel for market data...");
                await SendMessageAsync(client, "{\"type\":\"CHANNEL_REQUEST\",\"channel\":1,\"service\":\"FEED\",\"parameters\":{\"contract\":\"AUTO\"}}");

                Console.WriteLine("[Info] Subscribing to SPY candles...");
                long fromTimex = DateTimeOffset.UtcNow.AddDays(-4).ToUnixTimeSeconds();
                await SendMessageAsync(client, $"{{\"type\":\"FEED_SUBSCRIPTION\",\"channel\":1,\"reset\":true,\"add\":[{{\"type\":\"Candle\",\"symbol\":\"SPY{{=1d}}\",\"fromTime\":{fromTimex}}}]}}");

                Console.WriteLine("[Info] Waiting for market data...");
                await ReceiveMessagesAsync(client);

                //-------------
                /*
                Console.WriteLine("[Info] Connecting to WebSocket...");
                await client.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

                Console.WriteLine("[Info] Connected. Sending SETUP...");
                await SendMessageAsync(client, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

                Console.WriteLine("[Info] Authorizing...");
                await SendMessageAsync(client, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{_apiQuoteToken}\"}}");

                Console.WriteLine("[Info] Requesting historical data...");
                long fromUnixTime = ((DateTimeOffset)fromTime).ToUnixTimeSeconds();
                long toUnixTime = ((DateTimeOffset)toTime).ToUnixTimeSeconds();
                //await SendMessageAsync(client, $"{{\"type\":\"HISTORICAL_DATA_REQUEST\",\"channel\":1,\"symbol\":\"{symbol}\",\"fromTime\":{fromUnixTime},\"toTime\":{toUnixTime},\"timeframe\":\"{timeframe}\",\"interval\":{interval}}}");

                await SendMessageAsync(client, $"{{\"type\":\"FEED_SUBSCRIPTION\",\"channel\":1,\"reset\":true,\"add\":[{{\"type\":\"Candle\",\"symbol\":\"SPY{{=1d}}\",\"fromTime\":{fromUnixTime}}}]}}");
                */

                Console.WriteLine("[Info] Receiving historical data...");
                var buffer = new byte[8192];
                while (client.State == WebSocketState.Open)
                {
                    var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"\n[Received] {message}");

                    /*
                    var dataPoints = JsonSerializer.Deserialize<List<QuoteDataHistory.DataPoint>>(message);
                    if (dataPoints != null)
                    {
                        foreach (var dataPoint in dataPoints)
                        {
                            quoteDataHistory.AppendData(dataPoint);
                        }
                    }*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }

            return quoteDataHistory;
        }

        public void PatchHistoricData(QuoteDataHistory quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        public void StremLiveMarketData(QuoteDataHistory quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        public void CloseWsConnection()
        {
            throw new NotImplementedException();
        }

        
    }
}
