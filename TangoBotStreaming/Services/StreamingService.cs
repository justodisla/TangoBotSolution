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

        public async Task<QuoteDataHistory> StreamHistoricDataAsync(string symbol, DateTime fromTime, DateTime toTime, Timeframe timeframe = Timeframe.Daily, int interval = 1)
        {
            var quoteDataHistory = new QuoteDataHistory();
            using var client = new ClientWebSocket();

            // Use the correct candle type format
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
                await client.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

                Console.WriteLine("[Info] Connected. Sending SETUP...");
                await SendMessageAsync(client, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

                Console.WriteLine("[Info] Authorizing...");
                await SendMessageAsync(client, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{_apiQuoteToken}\"}}");

                Console.WriteLine("[Info] Opening channel for market data...");
                await SendMessageAsync(client, "{\"type\":\"CHANNEL_REQUEST\",\"channel\":1,\"service\":\"FEED\",\"parameters\":{\"contract\":\"AUTO\"}}");

                Console.WriteLine($"[Info] Subscribing to {symbol} candles...");

                long fromUnixTime = ((DateTimeOffset)fromTime).ToUnixTimeSeconds();
                await SendMessageAsync(client, $"{{\"type\":\"FEED_SUBSCRIPTION\",\"channel\":1,\"reset\":true,\"add\":[{{\"type\":\"Candle\",\"symbol\":\"{symbol}{{={candleType}}}\",\"fromTime\":{fromUnixTime}}}]}}");

                Console.WriteLine("[Info] Waiting for market data...");
                await ReceiveMessagesAsync(client, fromTime, toTime, quoteDataHistory);
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

        public void PatchHistoricData(QuoteDataHistory quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        public void StreamLiveMarketData(QuoteDataHistory quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        public void CloseWsConnection()
        {
            throw new NotImplementedException();
        }

        private async Task SendMessageAsync(ClientWebSocket client, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"[Sent] {message}");
        }

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

                    // Check if the quoteDataHistory has been fully populated
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

        private bool IsQuoteDataHistoryPopulated(QuoteDataHistory quoteDataHistory, DateTime fromTime, DateTime toTime)
        {
            // Define the time range for the checks
            var fromTimeRangeStart = fromTime.AddDays(-4);
            var fromTimeRangeEnd = fromTime.AddDays(4);
            var toTimeRangeStart = toTime.AddDays(-4);
            var toTimeRangeEnd = toTime.AddDays(4);

            // Check if there is a data point within four days of the fromTime
            bool hasFromTimeDataPoint = quoteDataHistory.DataPoints.Any(dp => dp.Time >= fromTimeRangeStart && dp.Time <= fromTimeRangeEnd);

            // Check if there is a data point within four days of the toTime
            bool hasToTimeDataPoint = quoteDataHistory.DataPoints.Any(dp => dp.Time >= toTimeRangeStart && dp.Time <= toTimeRangeEnd);

            // Return true if both conditions are met
            return hasFromTimeDataPoint && hasToTimeDataPoint;
        }



        private void ProcessMessage(string message, DateTime fromTime, DateTime toTime, QuoteDataHistory quoteDataHistory)
        {
            Console.WriteLine($"[Info] Processing message  {message}");

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

                        // Exclude data points that fall on the current day
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
    }
}
