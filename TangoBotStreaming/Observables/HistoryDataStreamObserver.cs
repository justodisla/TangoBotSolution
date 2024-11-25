using HttpClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TangoBotAPI.Streaming;

namespace TangoBotStreaming.Observables
{
    public class HistoryDataStreamObserver : IObserver<HistoricDataReceivedEvent>
    {
        private readonly HashSet<DateTime> _receivedDates = new();
        private readonly QuoteDataHistory _quoteDataHistory;
        private bool _isHistoricalDataComplete = false;

        public HistoryDataStreamObserver()
        {
            _quoteDataHistory = new QuoteDataHistory();
        }

        public void OnCompleted()
        {
            // Handle the completion of the data stream
            Console.WriteLine("HistoryDataStreamObserver: Data stream completed.");
        }

        public void OnError(Exception error)
        {
            // Handle any errors that occur during the data stream
            Console.WriteLine($"HistoryDataStreamObserver: An error occurred: {error.Message}");
        }

        public void OnNext(HistoricDataReceivedEvent value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ReceivedData))
            {
                // Process the received data
                var jsonDocument = JsonDocument.Parse(value.ReceivedData);
                var root = jsonDocument.RootElement;

                int counter = 0;

                if (root.GetProperty("type").GetString() == "FEED_DATA")
                {
                    var dataArray = root.GetProperty("data").EnumerateArray();
                    foreach (var data in dataArray)
                    {
                        if (data.GetProperty("eventType").GetString() == "Candle")
                        {
                            //Turn data into a QuoteDataHistory.DataPoint object
                            QuoteDataHistory.DataPoint quoteDataHistoryDataPoint = null;
                            try
                            {
                                if (data.GetProperty("open").ValueKind == JsonValueKind.Number &&
                                        data.GetProperty("high").ValueKind == JsonValueKind.Number &&
                                        data.GetProperty("low").ValueKind == JsonValueKind.Number &&
                                        data.GetProperty("close").ValueKind == JsonValueKind.Number)
                                {
                                    quoteDataHistoryDataPoint = new QuoteDataHistory.DataPoint(
                                        data.GetProperty("open").GetDecimal(),
                                        data.GetProperty("high").GetDecimal(),
                                        data.GetProperty("low").GetDecimal(),
                                        data.GetProperty("close").GetDecimal(),
                                        DateTimeOffset.FromUnixTimeMilliseconds(data.GetProperty("time").GetInt64()).UtcDateTime,
                                        data.GetProperty("volume").GetDouble(),
                                        GetDoubleOrDefault(data, "vwap"),
                                        GetDoubleOrDefault(data, "bidVolume"),
                                        GetDoubleOrDefault(data, "askVolume"),
                                        GetDoubleOrDefault(data, "impVolatility"));

                                    _quoteDataHistory.AppendData(quoteDataHistoryDataPoint);
                                }
                                else
                                {
                                    // Handle the case where the expected properties are not numbers
                                    //Console.WriteLine("Received data with incorrect types.");
                                }

                                //Console.WriteLine($"[Info] date: {quoteDataHistoryDataPoint.Time} counter:{counter} Received historical data for \n{data.ToString()}\n");

                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            var eventTime = DateTimeOffset.FromUnixTimeMilliseconds(data.GetProperty("time").GetInt64()).UtcDateTime;

                            //Console.WriteLine($"[Info] date: {eventTime} counter:{counter} Received historical data for \n{data.ToString()}\n");
                            counter++;

                            // Check if the event time is already in the received dates
                            if (_receivedDates.Contains(eventTime))
                            {
                                _isHistoricalDataComplete = true;
                                //Console.WriteLine("[Info] Historical data is complete. Closing connection.");
                                // Close the WebSocket connection
                                // Note: You need to have a reference to the WebSocket client to close it here
                                // _websocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Historical data received", CancellationToken.None);
                                break;
                            }
                            else
                            {
                                _receivedDates.Add(eventTime);
                            }
                        }
                    }
                }

            }
        }

        private double GetDoubleOrDefault(JsonElement data, string propertyName)
        {
            if (data.TryGetProperty(propertyName, out JsonElement propertyValue) && propertyValue.ValueKind == JsonValueKind.Number)
            {
                return propertyValue.GetDouble();
            }
            return 0.0;
        }

        public bool IsHistoricalDataComplete => _isHistoricalDataComplete;
    }
}
