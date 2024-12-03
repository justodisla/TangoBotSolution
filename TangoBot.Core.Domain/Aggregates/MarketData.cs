using System;
using System.Collections.Generic;
using System.Linq;
using TangoBot.Core.Api2;

namespace TangoBot.Core.Domain.Aggregates
{
    public class MarketData : IMarketData
    {
        private List<DataPoint> _dataPoints;
        private long _currentIndex;
        private ObservableManager<MarketDataEvent> _observableManager;

        public MarketData(string symbol, DateTime startDate, DateTime endDate)
        {
            Symbol = symbol;
            StartDate = startDate;
            EndDate = endDate;
            _currentIndex = 0;
        }

        public string Symbol { get; private set; }

        public List<DataPoint> DataPoints => _dataPoints;

        public DataPoint Current => _dataPoints[(int)_currentIndex];

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public long CurrentIndex => _currentIndex;

        public void AttachIndicator(string indicatorName, KeyValuePair<string, double>[] parameters = null)
        {
            // Example implementation of attaching an indicator
            Console.WriteLine($"Attaching indicator: {indicatorName}");
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    Console.WriteLine($"Parameter: {param.Key} = {param.Value}");
                }
            }
        }

        public void Compute()
        {
            // Example implementation of a compute method
            Console.WriteLine("Computing market data...");
        }

        public long GetCount()
        {
            return _dataPoints.Count;
        }

        public void Move(int offSet)
        {
            long newIndex = _currentIndex + offSet;
            if (newIndex >= 0 && newIndex < _dataPoints.Count)
            {
                _currentIndex = newIndex;
            }
        }

        public void MoveNext()
        {
            if (_currentIndex < _dataPoints.Count - 1)
            {
                _currentIndex++;
            }
        }

        public void Reset()
        {
            _currentIndex = 0;
        }

        public DataPoint Step(int offset = 1)
        {
            Move(offset);
            return Current;
        }

        public DataPoint StepToFirst(int offset = 0)
        {
            _currentIndex = 0 + offset;
            return Current;
        }

        public DataPoint StepToLast(int offset = 0)
        {
            _currentIndex = _dataPoints.Count - 1 + offset;
            return Current;
        }

        public IDisposable Subscribe(IObserver<MarketDataEvent> observer)
        {
            // Example implementation of a subscribe method
            return _observableManager.Subscribe(observer);
        }
    }
}




