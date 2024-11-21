using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Streaming;
using TangoBotAPI.Toolkit;

namespace TangoBotStreaming.Services
{
    /// <summary>
    /// Service for streaming market data.
    /// </summary>
    public class StreamingService : IStreamService<QuoteDataHistory>
    {
        private readonly string _webSocketUrl;
        private readonly string _apiQuoteToken;
        private ClientWebSocket _clientWebSocket;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingService"/> class.
        /// </summary>
        public StreamingService()
        {
            _webSocketUrl = TangoBotServiceProvider.GetService<IConfigurationProvider>()
                .GetConfigurationValue(Constants.DX_LINK_WS_URL);

            _apiQuoteToken = TangoBotServiceProvider.GetService<IConfigurationProvider>()
                .GetConfigurationValue(Constants.STREAMING_AUTH_TOKEN);
        }

        /// <inheritdoc />
        public async Task<QuoteDataHistory> StreamHistoricDataAsync(string symbol, DateTime fromTime, DateTime toTime, Timeframe timeframe = Timeframe.Daily, int interval = 1)
        {
            var quoteDataHistory = new QuoteDataHistory();
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);


            string candleType = timeframe switch
            {
                Timeframe.OneHour => "1h",
                Timeframe.FourHour => "4h",
                Timeframe.Daily => "1d",
                Timeframe.Weekly => "1w",
                Timeframe.Monthly => "1m",
                Timeframe.Year => "1y",
                _ => throw new ArgumentOutOfRangeException(nameof(timeframe), $"Unsupported timeframe: {timeframe}")
            };

            try
            {
                Console.WriteLine("[Info] Connecting to WebSocket...");
                await _clientWebSocket.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

                Console.WriteLine("[Info] Connected. Sending SETUP...");
                await SendMessageAsync(_clientWebSocket, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

                Console.WriteLine("[Info] Authorizing...");
                await SendMessageAsync(_clientWebSocket, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{_apiQuoteToken}\"}}");

                Console.WriteLine("[Info] Opening channel for market data...");
                await SendMessageAsync(_clientWebSocket, "{\"type\":\"CHANNEL_REQUEST\",\"channel\":1,\"service\":\"FEED\",\"parameters\":{\"contract\":\"AUTO\"}}");

                Console.WriteLine($"[Info] Subscribing to {symbol} candles...");

                long fromUnixTime = ((DateTimeOffset)fromTime).ToUnixTimeSeconds();
                await SendMessageAsync(_clientWebSocket, $"{{\"type\":\"FEED_SUBSCRIPTION\",\"channel\":1,\"reset\":true,\"add\":[{{\"type\":\"Candle\",\"symbol\":\"{symbol}{{={candleType}}}\",\"fromTime\":{fromUnixTime}}}]}}");

                Console.WriteLine("[Info] Waiting for market data...");
                await ReceiveMessagesAsync(_clientWebSocket, fromTime, toTime, quoteDataHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }

            Console.WriteLine("\n\n[Info] Data fully received. Closing WebSocket connection...");

            Console.WriteLine(quoteDataHistory.DataPoints.Count);

            foreach (var dataPoint in quoteDataHistory.DataPoints)
            {
                Console.WriteLine($"{dataPoint.Time} - {dataPoint.Close}");
            }

            return quoteDataHistory;
        }

        /// <inheritdoc />
        public void PatchHistoricData(QuoteDataHistory quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void StreamLiveMarketData(QuoteDataHistory quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void CloseWsConnection()
        {
            if (_clientWebSocket != null && _clientWebSocket.State == WebSocketState.Open)
            {
                _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None).Wait();
                Console.WriteLine("[Info] WebSocket connection closed.");
            }
            else
            {
                Console.WriteLine("[Info] WebSocket connection is already closed or not initialized.");
            }
        }

        #region Auxiliary methods

        /// <summary>
        /// Sends a message asynchronously over the WebSocket connection.
        /// </summary>
        /// <param name="client">The WebSocket client.</param>
        /// <param name="message">The message to send.</param>
        private async Task SendMessageAsync(ClientWebSocket client, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"[Sent] {message}");
        }

        /// <summary>
        /// Receives messages asynchronously from the WebSocket connection.
        /// </summary>
        /// <param name="client">The WebSocket client.</param>
        /// <param name="fromTime">The start time for the data stream.</param>
        /// <param name="toTime">The end time for the data stream.</param>
        /// <param name="quoteDataHistory">The data object to store the received data.</param>
        private async Task ReceiveMessagesAsync(ClientWebSocket client, DateTime fromTime, DateTime toTime, QuoteDataHistory quoteDataHistory)
        {
            var buffer = new byte[1024 * 4];
            var messageBuilder = new StringBuilder();

            while (client.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                do
                {
                    result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        return;
                    }

                    var messagePart = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    messageBuilder.Append(messagePart);

                } while (!result.EndOfMessage);

                var message = messageBuilder.ToString();
                messageBuilder.Clear();

                Console.WriteLine($"[Received] {message}");
                try
                {
                    ProcessMessage(message, fromTime, toTime, quoteDataHistory);

                    if (IsQuoteDataHistoryPopulated(quoteDataHistory, fromTime, toTime))
                    {
                        Console.WriteLine("[Info] Data fully received. Closing WebSocket connection...");
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Data fully received", CancellationToken.None);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Exception while processing message: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Checks if the quote data history is fully populated within the specified time range.
        /// </summary>
        /// <param name="quoteDataHistory">The data object to check.</param>
        /// <param name="fromTime">The start time for the data stream.</param>
        /// <param name="toTime">The end time for the data stream.</param>
        /// <returns>True if the data object is fully populated; otherwise, false.</returns>
        private bool IsQuoteDataHistoryPopulated(QuoteDataHistory quoteDataHistory, DateTime fromTime, DateTime toTime)
        {
            var fromTimeRangeStart = fromTime.AddDays(-4);
            var fromTimeRangeEnd = fromTime.AddDays(4);
            var toTimeRangeStart = toTime.AddDays(-4);
            var toTimeRangeEnd = toTime.AddDays(4);

            bool hasFromTimeDataPoint = quoteDataHistory.DataPoints.Any(dp => dp.Time >= fromTimeRangeStart && dp.Time <= fromTimeRangeEnd);
            bool hasToTimeDataPoint = quoteDataHistory.DataPoints.Any(dp => dp.Time >= toTimeRangeStart && dp.Time <= toTimeRangeEnd);

            return hasFromTimeDataPoint && hasToTimeDataPoint;
        }
        /// <summary>
        /// Processes a received message and updates the quote data history.
        /// </summary>
        /// <param name="message">The received message.</param>
        /// <param name="fromTime">The start time for the data stream.</param>
        /// <param name="toTime">The end time for the data stream.</param>
        /// <param name="quoteDataHistory">The data object to store the received data.</param>
        private void ProcessMessage(string message, DateTime fromTime, DateTime toTime, QuoteDataHistory quoteDataHistory)
        {
            Console.WriteLine($"[Info] Processing message {message}");

            var jsonDocument = JsonDocument.Parse(message);
            var root = jsonDocument.RootElement;

            if (root.GetProperty("type").GetString() == "FEED_DATA")
            {
                var dataArray = root.GetProperty("data").EnumerateArray();
                foreach (var data in dataArray)
                {
                    if (data.GetProperty("eventType").GetString() == "Candle")
                    {
                        var eventTime = DateTimeOffset.FromUnixTimeMilliseconds(data.GetProperty("time").GetInt64()).UtcDateTime;
                        var currentDate = DateTime.UtcNow.Date;

                        if (eventTime.Date != currentDate && eventTime >= fromTime && eventTime <= toTime)
                        {
                            try
                            {
                                var open = data.GetProperty("open").GetDecimal();
                                var high = data.GetProperty("high").GetDecimal();
                                var low = data.GetProperty("low").GetDecimal();
                                var close = data.GetProperty("close").GetDecimal();
                                var volume = data.GetProperty("volume").GetDouble();
                                var impVolatility = ResolveDoubleFromProperty(data, "impVolatility");
                                var vwap = ResolveDoubleFromProperty(data, "vwap");
                                var bidVolume = ResolveDoubleFromProperty(data, "bidVolume");
                                var askVolume = ResolveDoubleFromProperty(data, "askVolume");

                                var dataPoint = new QuoteDataHistory.DataPoint(open, high, low, close, eventTime, volume, vwap, bidVolume, askVolume, impVolatility);
                                quoteDataHistory.AppendData(dataPoint);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[Error] Exception while processing message: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolves a double value from a JSON property.
        /// </summary>
        /// <param name="data">The JSON element containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The resolved double value.</returns>
        private double ResolveDoubleFromProperty(JsonElement data, string propertyName)
        {
            try
            {
                var tmpIV = data.GetProperty("impVolatility").ToString();
                if (tmpIV == "NaN")
                {
                    return 0.0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var tmpValue = data.GetProperty(propertyName).GetDouble();
                return tmpValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception while processing message: {ex.Message}");
            }
            return 0.0;
        }

        /// <summary>
        /// Validates the streaming authentication token.
        /// </summary>
        /// <param name="streamingToken">The streaming token to validate.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        public async Task<bool> IsStreamingAuthTokenValid(string streamingToken)
        {
            using var client = new ClientWebSocket();

            Console.WriteLine("[Info] Connecting to WebSocket...");
            await client.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

            Console.WriteLine("[Info] Connected. Sending SETUP...");
            await SendMessageAsync(client, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

            Console.WriteLine("[Info] Authorizing...");
            await SendMessageAsync(client, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{streamingToken}\"}}");

            var buffer = new byte[1024 * 4];
            var messageBuilder = new StringBuilder();

            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var messagePart = Encoding.UTF8.GetString(buffer, 0, result.Count);
                messageBuilder.Append(messagePart);

                if (result.EndOfMessage)
                {
                    var message = messageBuilder.ToString();
                    messageBuilder.Clear();

                    Console.WriteLine($"[Received] {message}");

                    try
                    {
                        var jsonDocument = JsonDocument.Parse(message);
                        var root = jsonDocument.RootElement;

                        if (root.GetProperty("type").GetString() == "AUTH_STATE" &&
                            root.GetProperty("state").GetString() == "AUTHORIZED")
                        {
                            Console.WriteLine("[Info] Authorization successful.");
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Error] Exception while processing message: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("[Info] Authorization failed.");
            return false;
        }

        #endregion
    }
}
