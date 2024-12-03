using System;
using System.Collections.Generic;
using System.Linq;
using TangoBot.Core.Api2;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Streaming;

namespace TangoBot.Core.Domain.Aggregates
{
    public class MarketData : IMarketData
    {
        private const string THROTTLE_PAUSED_RESUMED = "THROTTLE_PAUSED_RESUMED";
        private const string POINTER_MOVED = "POINTER_MOVED";
        private const string THROTTLE_STOPPED = "THROTTLE_STOPPED";

        // Contains the data points for the market data
        private readonly List<DataPoint> _dataPoints;

        // Contains the current index of the market data
        private long _currentIndex = 0;

        // A property that wraps the _currentIndex field
        public long CurrentIndex
        {
            get => _currentIndex; private set
            {
                if (_currentIndex != value)
                {
                    _currentIndex = value;
                    _observableManager
                                .Notify(new MarketDataEvent(POINTER_MOVED, Current));
                }
            }
        }

        private bool _paused = false;
        private long _throttle = -1;
        private bool _stopped = false;
        private TimeFrame _timeFrame;

        private ObservableManager<MarketDataEvent> _observableManager;

        public MarketData(string symbol, DateTime startDate, DateTime endDate, TimeFrame timeFrame = TimeFrame.Day)
        {
            Symbol = symbol;
            StartDate = startDate;
            EndDate = endDate;
            _currentIndex = 0;
            _timeFrame = timeFrame;

            _observableManager = new ObservableManager<MarketDataEvent>();

            //var _thclient = ServiceLocator.GetSingletonService<IStreamingService>();

            //Load the data
            //Attach the indicators
            //Merge the data
        }

        public string Symbol { get; private set; }

        public List<DataPoint> DataPoints => _dataPoints;

        public DataPoint Current => _dataPoints[(int)CurrentIndex];

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

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

        public void Load()
        {
            // Establish a connection to the data source
            // 
            throw new NotImplementedException();
        }

        public void Move(int offSet)
        {
            long newIndex = CurrentIndex + offSet;
            if (newIndex >= 0 && newIndex < _dataPoints.Count)
            {
                CurrentIndex = newIndex;
            }
        }

        public void MoveNext()
        {
            if (CurrentIndex < _dataPoints.Count - 1)
            {
                CurrentIndex++;
            }
        }

        public void PauseResume()
        {
            _paused = !_paused;
            _observableManager
                            .Notify(new MarketDataEvent(THROTTLE_PAUSED_RESUMED, Current));
        }

        public void Reset()
        {
            CurrentIndex = 0;
        }

        public DataPoint Step(int offset = 1)
        {
            Move(offset);
            return Current;
        }

        public DataPoint StepToFirst(int offset = 0)
        {
            CurrentIndex = 0 + offset;
            return Current;
        }

        public DataPoint StepToLast(int offset = 0)
        {
            CurrentIndex = _dataPoints.Count - 1 + offset;
            return Current;
        }

        public void Stop(bool reset = true)
        {
            _stopped = true;
        }

        public IDisposable Subscribe(IObserver<MarketDataEvent> observer)
        {
            // Example implementation of a subscribe method
            return _observableManager.Subscribe(observer);
        }


        public async void Throttle(int milliseconds, bool reset = false, int offset = 1)
        {
            _throttle = milliseconds;

            if (reset)
            {
                Reset();
            }

            if (_throttle != -1)
            {
                while (!_stopped)
                {
                    await Task.Delay(milliseconds);

                    if (!_paused)
                    {
                        Step(offset);
                    }
                }
                _observableManager.Notify(new MarketDataEvent(THROTTLE_STOPPED, Current));
            }
        }
    }
}




