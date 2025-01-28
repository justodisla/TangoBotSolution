using System;
using System.Collections.Generic;
using System.Linq;
using TangoBot.Core.Api2;
using TangoBotApi.Common;
using TangoBotApi.Infrastructure;
using TangoBotApi.Services.Streaming;

namespace TangoBot.Core.Domain.Aggregates
{
    /// <summary>
    /// Manages live market data interactions and queries.
    /// </summary>
    public class LiveMarketDataManager : IMarketDataManager
    {
        private const string THROTTLE_PAUSED_RESUMED = "THROTTLE_PAUSED_RESUMED";
        private const string POINTER_MOVED = "POINTER_MOVED";
        private const string THROTTLE_STOPPED = "THROTTLE_STOPPED";

        // Contains the data points for the market data
        private readonly List<DataPoint> _dataPoints;

        // Contains the current index of the market data
        private long _currentIndex = 0;

        /// <summary>
        /// Gets the current index of the market data.
        /// </summary>
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

        private ObservableHelper<MarketDataEvent> _observableManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveMarketDataManager"/> class.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <param name="startDate">The start date of the market data.</param>
        /// <param name="endDate">The end date of the market data.</param>
        /// <param name="timeFrame">The time frame of the market data.</param>
        public LiveMarketDataManager(string symbol, DateTime startDate, DateTime endDate, TimeFrame timeFrame = TimeFrame.Day)
        {
            Symbol = symbol;
            StartDate = startDate;
            EndDate = endDate;
            _currentIndex = 0;
            _timeFrame = timeFrame;

            _observableManager = new ObservableHelper<MarketDataEvent>();

            _dataPoints = new();

            //var _thclient = ServiceLocator.GetSingletonService<IStreamingService>();

            //Load the data
            //Attach the indicators
            //Merge the data
        }

        /// <summary>
        /// Gets the symbol of the market data.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Gets the list of data points that represent the market data.
        /// </summary>
        public List<DataPoint> DataPoints => _dataPoints;

        /// <summary>
        /// Gets the current data point in the data points list.
        /// </summary>
        public DataPoint Current => _dataPoints[(int)CurrentIndex];

        /// <summary>
        /// Gets the start date of the data points list.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Gets the end date of the data points list.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Attaches indicators to the market data.
        /// </summary>
        /// <param name="indicatorName">The name of the indicator.</param>
        /// <param name="parameters">The parameters of the indicator.</param>
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

        /// <summary>
        /// Computes the market data.
        /// </summary>
        public void Compute()
        {
            // Example implementation of a compute method
            Console.WriteLine("Computing market data...");
        }

        /// <summary>
        /// Returns the number of elements in the data points list.
        /// </summary>
        /// <returns>The count of data points.</returns>
        public long GetCount()
        {
            return _dataPoints.Count;
        }

        /// <summary>
        /// Loads the data from the server.
        /// </summary>
        public void Load()
        {
            // Establish a connection to the data source
            // 
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the pointer to a given offset.
        /// </summary>
        /// <param name="offSet">The offset to move the pointer.</param>
        public void Move(int offSet)
        {
            long newIndex = CurrentIndex + offSet;
            if (newIndex >= 0 && newIndex < _dataPoints.Count)
            {
                CurrentIndex = newIndex;
            }
        }

        /// <summary>
        /// Moves to the next element in the data points list.
        /// </summary>
        public void MoveNext()
        {
            if (CurrentIndex < _dataPoints.Count - 1)
            {
                CurrentIndex++;
            }
        }

        /// <summary>
        /// Pauses or resumes the throttle.
        /// </summary>
        public void PauseResume()
        {
            _paused = !_paused;
            _observableManager
                            .Notify(new MarketDataEvent(THROTTLE_PAUSED_RESUMED, Current));
        }

        /// <summary>
        /// Resets the current element in the data points list to the first element.
        /// </summary>
        public void Reset()
        {
            CurrentIndex = 0;
        }

        /// <summary>
        /// Returns the current element in the data points list and moves the pointer to the next element.
        /// </summary>
        /// <param name="offset">The offset to move the pointer.</param>
        /// <returns>The current data point.</returns>
        public DataPoint Step(int offset = 1)
        {
            Move(offset);
            return Current;
        }

        /// <summary>
        /// Moves the pointer to the first element in the data points list.
        /// </summary>
        /// <param name="offset">The offset to move the pointer.</param>
        /// <returns>The first data point.</returns>
        public DataPoint StepToFirst(int offset = 0)
        {
            CurrentIndex = 0 + offset;
            return Current;
        }

        /// <summary>
        /// Moves to the last element in the data points list.
        /// </summary>
        /// <param name="offset">The offset to move the pointer.</param>
        /// <returns>The last data point.</returns>
        public DataPoint StepToLast(int offset = 0)
        {
            CurrentIndex = _dataPoints.Count - 1 + offset;
            return Current;
        }

        /// <summary>
        /// Stops the throttle and optionally resets the pointer to the first data point.
        /// </summary>
        /// <param name="reset">Whether to reset the pointer to the first data point.</param>
        public void Stop(bool reset = true)
        {
            _stopped = true;
        }

        /// <summary>
        /// Subscribes an observer to the market data events.
        /// </summary>
        /// <param name="observer">The observer to subscribe.</param>
        /// <returns>A disposable object to unsubscribe the observer.</returns>
        public IDisposable Subscribe(IObserver<MarketDataEvent> observer)
        {
            // Example implementation of a subscribe method
            return _observableManager.Subscribe(observer);
        }

        /// <summary>
        /// Throttles the market data at a given rate.
        /// </summary>
        /// <param name="milliseconds">The rate in milliseconds.</param>
        /// <param name="reset">Whether to reset the pointer to the first data point.</param>
        /// <param name="offset">The offset to move the pointer.</param>
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
