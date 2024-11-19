using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Streaming
{
    /// <summary>
    /// QuoteDataHistory is a class that holds the historic and live market data for a given symbol.
    /// along with the timeframe and interval of the data, the symbol and the time range of the data.
    /// </summary>
    public class QuoteDataHistory
    {
        private readonly List<DataPoint> _data = new();

        public void AppendData(DataPoint dataPoint)
        {
            if (_data.Any())
            {
                dataPoint.ForwardIndex = _data.Last().ForwardIndex + 1;
            }
            else
            {
                dataPoint.ForwardIndex = 0;
            }

            _data.Add(dataPoint);
        }

        public class DataPoint
        {
            public DataPoint(decimal open, decimal high, decimal low, decimal close, DateTime time, double volume)
            {
                Open = open;
                High = high;
                Low = low;
                Close = close;
                Time = time;
                Volume = volume;
            }

            public decimal Open { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Close { get; set; }
            public DateTime Time { get; set; }
            public double Volume { get; set; }
            internal double ForwardIndex { get; set; }
        }
    }
}
