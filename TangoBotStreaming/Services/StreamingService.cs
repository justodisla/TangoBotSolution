using System;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TangoBot.API.Configuration;
using TangoBot.API.Observable;
using TangoBot.API.Streaming;
using TangoBot.API.TokenManagement;
using TangoBot.API.Toolkit;
using TangoBot.DependecyInjection;
using TangoBotStreaming.Observables;
using TangoBotStreaming.Utilities;

namespace TangoBot.Streaming.Services
{
    /// <summary>
    /// Service for streaming market data.
    /// </summary>
    public class StreamingService : IStreamingService, IObservable<HistoricDataReceivedEvent>
    {
        private readonly string _webSocketUrl;
        private string? _apiQuoteToken;
        private readonly ClientWebSocket _websocketClient;

        private readonly ObserverManager<HistoricDataReceivedEvent> _observerManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingService"/> class.
        /// </summary>
        public StreamingService()
        {
            _observerManager = new ObserverManager<HistoricDataReceivedEvent>();

            _webSocketUrl = TangoBotServiceLocator.GetSingletonService<IConfigurationProvider>()
                .GetConfigurationValue(Constants.DX_LINK_WS_URL);

            _websocketClient = new ClientWebSocket();

            Subscribe(new HistoryDataStreamObserver());
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

            var _tokenProvider = TangoBotServiceLocator.GetSingletonService<ITokenProvider>()
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

                    if (result.MessageType == WebSocketMessageType.Close)//Close message received from server
                    {
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        return;
                    }

                    var messagePart = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    messageBuilder.Append(messagePart);

                } while (!result.EndOfMessage);//A message is complete when the EndOfMessage is true

                WsResponse wsresponse = new(messageBuilder.ToString());

                //If the message is not a FEED_DATA message, skip
                if (wsresponse.Type != "FEED_DATA")
                {
                    continue;
                }

                //The response is of type FEED_DATA. Let's extract the Candle event type. FEED_DATA is a list of events.
                wsresponse.Data.ForEach(item =>
                {
                    if (item.EventType == "Candle" && !isDataFullyReceived)//Check if the event is a Candle event and the data is not fully received
                    {
                        //Thread.Sleep(10);

                        //Extract the time of the event
                        var timeInUnixMss = item.Time;
                        var timeOfEvent = DateTimeOffset.FromUnixTimeMilliseconds(timeInUnixMss).UtcDateTime;

                        //If timeOfEvent is earlier than yesterday do not execute the next two statements
                        if (timeOfEvent.Date < DateTime.UtcNow.Date.AddDays(-5))
                        {
                            Console.WriteLine($"[Received Date] {item.High}");
                            Console.WriteLine($"[Received Date] {timeOfEvent.ToString()}\n");

                            //Notify the observers
                            _observerManager.Notify(new HistoricDataReceivedEvent(item));
                        }
                        else
                        {
                            //Console.WriteLine($"{timeOfEvent.ToString()} IS TOO EARLY");
                        }

                        //If timeOfEvent is older than 01-01-2015 break
                        if (timeOfEvent.Date < fromTime)
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

                //Send a heartbeat to the websocket
                await StreamingUtils.SendMessageAsync(client, "{\"type\":\"KEEPALIVE\",\"channel\":0}");

                // Thread.Sleep(1000);

            }

            //Close the WebSocket connection
            CloseWsConnection();

            //Notify the observers that the data stream is completed
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }

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
        private void CloseWsConnection()
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
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return _observerManager.Subscribe(observer);
        }

        #endregion

        #endregion

    }
}
