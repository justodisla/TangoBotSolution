﻿using System;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TangoBotAPI.Configuration;
using TangoBotAPI.DI;
using TangoBotAPI.Streaming;
using TangoBotAPI.TokenManagement;
using TangoBotAPI.Toolkit;
using TangoBotStreaming.Utilities;

namespace TangoBotStreaming.Services
{
    /// <summary>
    /// Service for streaming market data.
    /// </summary>
    public class StreamingService : IStreamingService, IObservable<HistoricDataReceivedEvent>
    {
        private readonly string _webSocketUrl;
        private string? _apiQuoteToken;
        private ClientWebSocket _websocketClient;

        private readonly ObserverManager<HistoricDataReceivedEvent> _observerManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingService"/> class.
        /// </summary>
        public StreamingService()
        {
            _observerManager = new ObserverManager<HistoricDataReceivedEvent>();

            _webSocketUrl = TangoBotServiceProvider.GetService<IConfigurationProvider>()
                .GetConfigurationValue(Constants.DX_LINK_WS_URL);

            _websocketClient = new ClientWebSocket();
        }

        /// <inheritdoc />
        public async void StreamHistoricDataAsync(
            string symbol,
            DateTime fromTime,
            DateTime toTime,
            Timeframe timeframe = Timeframe.Daily,
            int interval = 1)
        {

            //Define that the toDate is the previous day
            toTime = toTime.AddDays(-1);

            var _tokenProvider = TangoBotServiceProvider.GetService<ITokenProvider>()
                ?? throw new Exception("TokenProvider is null");

            _apiQuoteToken = _tokenProvider.GetValidStreamingToken().Result;

            await _websocketClient.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);

            //Correct the candleType
            string candleType = timeframe switch
            {
                Timeframe.OneHour => "1h",
                Timeframe.FourHour => "4h",
                Timeframe.Daily => "1d",
                Timeframe.Weekly => "1w",
                Timeframe.Monthly => "1m",
                Timeframe.Year => "1y",
                _ => throw new ArgumentOutOfRangeException(nameof(timeframe),
                $"Unsupported timeframe: {timeframe}")
            };

            try
            {
                #region Connect
                //Console.WriteLine("[Info] Connecting to WebSocket...");
                if (_websocketClient.State != WebSocketState.Open)
                {
                    await _websocketClient.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);
                    //Console.WriteLine("[Info] Connected.");
                }
                else
                {
                    //Console.WriteLine("[Info] WebSocket connection is already open.");
                }
                #endregion

                #region Prepare connection

                //Console.WriteLine("[Info] Connected. Sending SETUP...");
                await StreamingUtils.SendMessageAsync(_websocketClient, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

                //Console.WriteLine("[Info] Authorizing...");
                await StreamingUtils.SendMessageAsync(_websocketClient, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{_apiQuoteToken}\"}}");

                //Console.WriteLine("[Info] Opening channel for market data...");
                await StreamingUtils.SendMessageAsync(_websocketClient, "{\"type\":\"CHANNEL_REQUEST\",\"channel\":1,\"service\":\"FEED\",\"parameters\":{\"contract\":\"AUTO\"}}");

                //Console.WriteLine($"[Info] Subscribing to {symbol} candles...");

                long fromUnixTime = ((DateTimeOffset)fromTime).ToUnixTimeSeconds();
                await StreamingUtils.SendMessageAsync(_websocketClient, $"{{\"type\":\"FEED_SUBSCRIPTION\",\"channel\":1,\"reset\":true,\"add\":[{{\"type\":\"Candle\",\"symbol\":\"{symbol}{{={candleType}}}\",\"fromTime\":{fromUnixTime}}}]}}");

                #endregion

                //Console.WriteLine("[Info] Waiting for market data...");
                await ReceiveMessagesAsync(_websocketClient, fromTime, toTime);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task ReceiveMessagesAsync(ClientWebSocket client, DateTime fromTime, DateTime toTime)
        {

            Console.WriteLine("\n\n\n[Info] Receiving messages...");

            var buffer = new byte[1024 * 4];
            var messageBuilder = new StringBuilder();

            bool isDataFullyReceived = false;

            while (client.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                messageBuilder.Clear();
                do
                {
                    //Receives a message part and adds it to messageBuilder

                    result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        return;
                    }

                    var messagePart = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    messageBuilder.Append(messagePart);

                    //Console.WriteLine($"\n\n\n[Received] Message part of length: {messagePart.Length} \n{messagePart[..50]}\n[Received] Message part of length: {messagePart.Length}");

                } while (!result.EndOfMessage);//A message is complete when the EndOfMessage is true

                WsResponse wsresponse = new(messageBuilder.ToString());

                //If the message is not a FEED_DATA message, skip
                if (wsresponse.Type != "FEED_DATA")
                {
                    continue;
                }

                //In the data array filter events of type Candle
                wsresponse.Data.ForEach(item =>
                {
                    if (item.EventType == "Candle" && !isDataFullyReceived)
                    {
                        Thread.Sleep(10);

                        //Extract the time of the event
                        var timeInUnixMss = item.Time;
                        var timeOfEvent = DateTimeOffset.FromUnixTimeMilliseconds(timeInUnixMss).UtcDateTime;

                        //If timeOfEvent is earliear than yesterday do not execute the next two statements
                        if (timeOfEvent.Date < DateTime.UtcNow.Date.AddDays(-5))
                        {
                            Console.WriteLine($"[Received Date] {item.High}");
                            Console.WriteLine($"[Received Date] {timeOfEvent.ToString()}\n");

                            //Notify the observers
                            //var _historicDataReceivedEvent = new HistoricDataReceivedEvent(messageBuilder.ToString());

                        }
                        else
                        {
                            Console.WriteLine($"{timeOfEvent.ToString()} IS TOO EARLY");
                        }

                        //If timeOfEvent is older than 01-01-2015 break
                        if (timeOfEvent.Date < new DateTime(2023, 1, 1))
                        {
                            isDataFullyReceived = true;
                            return;
                        }

                        //if timeOfEvent ouside of the range defined by fromDate and toDate, skip
                        bool isTimeOfEventInRange = timeOfEvent.Date <= fromTime && timeOfEvent.Date >= toTime;

                        if (isTimeOfEventInRange)
                        {
                            //Console.WriteLine($"[Info] Skipping event outside of the range: {timeOfEvent.ToString()}");
                            return;
                        }

                        //Console.WriteLine($"[Info] Data in range: {timeOfEvent.ToString()}");

                    }
                });

                if (isDataFullyReceived)
                {
                    Console.WriteLine("[Info] Data fully received.");
                    break;
                }
                //Verify if within the date range

                //The message must be analized to find if it is a candle event



                //Console.WriteLine($"\n[Received] Message of length: {messageBuilder.ToString().Length} \n{messageBuilder.ToString()}\n[Received] Message of length: {messageBuilder.ToString().Length}\n\n");

                //Send a hartbeat to the websocket
                await StreamingUtils.SendMessageAsync(client, "{\"type\":\"KEEPALIVE\",\"channel\":0}");
                
                Thread.Sleep(1000);

            }

            //var message = messageBuilder.ToString();

            //Console.WriteLine($"\n\n\n[Received] {message}");

            return;

        }

        int count = 0;

        [Obsolete("This method is not used in the current implementation.")]
        /// <summary>
        /// Receives messages asynchronously from the WebSocket connection.
        /// </summary>
        /// <param name="client">The WebSocket client.</param>
        /// <param name="fromTime">The start time for the data stream.</param>
        /// <param name="toTime">The end time for the data stream.</param>
        /// <param name="quoteDataHistory">The data object to store the received data.</param>
        private async Task ReceiveMessagesAsyncx(ClientWebSocket client, DateTime fromTime, DateTime toTime)
        {
            Console.WriteLine("\n\n\n[Info] Receiving messages...");

            var buffer = new byte[1024 * 4];
            var messageBuilder = new StringBuilder();

            while (client.State == WebSocketState.Open)
            {
                //Gather the message chunks
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

                //Collect the chunks of the message received so far
                var message = messageBuilder.ToString();
                messageBuilder.Clear();

                //Cast the message to a WsResponse object
                var wsresponse = new WsResponse(message) ?? throw new Exception("Unable to create WsResponse instance");

                //Skip if the message is not a feed data message
                if (wsresponse.Data == null || wsresponse.Type != "FEED_DATA")
                {
                    continue;
                }

                if (count++ > 5)
                    continue;

                //Process what came in data
                foreach (WsResponse.DataItem item in wsresponse.Data)
                {
                    try
                    {
                        //Check if Candle event is after the toDate
                        var timeInUnixMss = wsresponse.Data[0].Time;
                        var timeOfEvent = DateTimeOffset.FromUnixTimeMilliseconds(timeInUnixMss).UtcDateTime;

                        //if timeOfEvent ouside of the range defined by fromDate and toDate, skip
                        bool isTimeOfEventInRange = timeOfEvent.Date <= fromTime && timeOfEvent.Date >= toTime;

                        if (isTimeOfEventInRange)
                        {
                            //Console.WriteLine($"[Info] Skipping event outside of the range: {timeOfEvent.ToString()}");
                            continue;
                        }

                        Console.WriteLine($"[Info] Data in range: {timeOfEvent.ToString()}");

                        // var eventTimex = DateTimeOffset.FromUnixTimeMilliseconds(item.Time).UtcDateTime;
                        //Console.WriteLine($"[Received Date] {eventTimex.ToString()}");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                continue;

                var currentDate = DateTime.UtcNow.Date;
                var timeInUnixMs = wsresponse.Data[0].Time;

                var eventTime = DateTimeOffset.FromUnixTimeMilliseconds(timeInUnixMs).UtcDateTime;
                if (eventTime.Date == currentDate)
                {
                    continue;
                }

                if (count++ > 20)
                {
                    foreach (var observer in _observers)
                    {
                        observer.OnCompleted();
                    }
                    CloseWsConnection();
                    break;
                }

                //Delegate the message processing to the observers
                var _historicDataReceivedEvent = new HistoricDataReceivedEvent(message);
                _observerManager.Notify(_historicDataReceivedEvent);

                //TODO: Detect when the historic data is fully received
                //TODO: Implement the logic to stop the streaming when the data is fully received
                //TODO: Implement the logic to close the WebSocket connection when the data is fully received

            }

            //Notify the observers that the data stream is completed

        }



        /// <inheritdoc />
        public void PatchHistoricData<T>(T quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void StreamLiveMarketData<T>(T quoteDataHistory)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void CloseWsConnection()
        {
            if (_websocketClient != null && _websocketClient.State == WebSocketState.Open)
            {
                _websocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None).Wait();
                Console.WriteLine("[Info] WebSocket connection closed.");
            }
            else
            {
                Console.WriteLine("[Info] WebSocket connection is already closed or not initialized.");
            }
        }

        #region Auxiliary methods


        [Obsolete("This method is not used in the current implementation.")]
        /// <summary>
        /// Checks if the quote data history is fully populated within the specified time range.
        /// </summary>
        /// <param name="quoteDataHistory">The data object to check.</param>
        /// <param name="fromTime">The start time for the data stream.</param>
        /// <param name="toTime">The end time for the data stream.</param>
        /// <returns>True if the data object is fully populated; otherwise, false.</returns>
        private static bool IsQuoteDataHistoryPopulated(QuoteDataHistory quoteDataHistory, DateTime fromTime, DateTime toTime)
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
            await StreamingUtils.SendMessageAsync(client, "{\"type\":\"SETUP\",\"channel\":0,\"version\":\"0.1-DXF-JS/0.3.0\",\"keepaliveTimeout\":60,\"acceptKeepaliveTimeout\":60}");

            Console.WriteLine("[Info] Authorizing...");
            await StreamingUtils.SendMessageAsync(client, $"{{\"type\":\"AUTH\",\"channel\":0,\"token\":\"{streamingToken}\"}}");

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

        #region Observable

        private readonly List<IObserver<HistoricDataReceivedEvent>> _observers = new List<IObserver<HistoricDataReceivedEvent>>();

        public IDisposable Subscribe(IObserver<HistoricDataReceivedEvent> observer)
        {
            if ((!_observers.Contains(observer)))
            {
                _observers.Add(observer);
            }
            return _observerManager.Subscribe(observer);
        }

        #endregion

        #endregion

    }
}
