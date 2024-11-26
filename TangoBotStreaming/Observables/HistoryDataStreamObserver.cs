using HttpClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TangoBotAPI.DI;
using TangoBotAPI.Persistence;
using TangoBotAPI.Streaming;

namespace TangoBotStreaming.Observables
{
    public class HistoryDataStreamObserver : IObserver<HistoricDataReceivedEvent>
    {
        private readonly HashSet<DateTime> _receivedDates = new();
        private readonly QuoteDataHistory _quoteDataHistory;
        private bool _isHistoricalDataComplete = false;
        private IPersistence? _persistence;

        public HistoryDataStreamObserver()
        {
            _quoteDataHistory = new QuoteDataHistory();
            TangoBotServiceProvider.AddService<IPersistence>(provider => new InMemoryPersistence(), typeof(InMemoryPersistence).Name);
            _persistence = TangoBotServiceProvider.GetTransientService<IPersistence>(typeof(InMemoryPersistence).Name);
        }

        public void OnCompleted()
        {
            if(_persistence == null)
            {
                throw new InvalidOperationException("Persistence service is not available.");
            }

            // Handle the completion of the data stream
            Console.WriteLine("HistoryDataStreamObserver: Data stream completed.");

            foreach (var dataPoint in _quoteDataHistory.DataPoints)
            {
                _persistence.CreateAsync(dataPoint).Wait();
            }
        }

        public void OnError(Exception error)
        {
            // Handle any errors that occur during the data stream
            Console.WriteLine($"HistoryDataStreamObserver: An error occurred: {error.Message}");
        }

        public void OnNext(HistoricDataReceivedEvent value)
        {
            Console.WriteLine("HistoryDataStreamObserver: Received data.");

            if (value.ReceivedData != null)
            {
                var dataItem = value.ReceivedData;

                // Convert DataItem to DataPoint
                var quoteDataHistoryDataPoint = new QuoteDataHistory.DataPoint(
                    dataItem.Open,
                    dataItem.High,
                    dataItem.Low,
                    dataItem.Close,
                    DateTimeOffset.FromUnixTimeMilliseconds(dataItem.Time).UtcDateTime,
                    dataItem.Volume,
                    dataItem.Vwap,
                    dataItem.BidVolume,
                    dataItem.AskVolume,
                    dataItem.ImpVolatility
                );

                // Append the converted DataPoint to QuoteDataHistory
                _quoteDataHistory.AppendData(quoteDataHistoryDataPoint);

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

