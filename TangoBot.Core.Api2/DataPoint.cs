using System;
using System.Collections.Generic;

namespace TangoBot.Core.Api2
{
    /// <summary>
    /// A datapoint of a MarketData object containing a list of key value pairs
    /// to represent the price data (OHLCV) of a given symbol at a given time
    /// plus the indicators values at that time like moving averages, RSI, etc.
    /// For instance if it has an indicator attachment like a moving average (SMA14) 
    /// then the key value pair would be SMA14: 123.45. The key is SMA14 and the value is 123.45
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Low price of the data point
        /// </summary>
        public double L { get; set; }

        /// <summary>
        /// High price of the data point
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// Open price of the data point
        /// </summary>
        public double O { get; set; }

        /// <summary>
        /// Close price of the data point
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// Volume of the data point
        /// </summary>
        public double V { get; set; }

        /// <summary>
        /// Timestamp of the data point
        /// </summary>
        public DateTime T { get; set; }

        /// <summary>
        /// Index of the data point in the list
        /// </summary>
        public double I { get; set; }

        /// <summary>
        /// List of key value pairs to represent the indicators values at that time
        /// for instance if it has an indicator attachment like a moving average (SMA14)
        /// one of the key value pairs would be SMA14: 123.45. The key is SMA14 and the value is 123.45
        /// </summary>
        public List<KeyValuePair<string, double>>? IndicatorEntries { get; set; }

        /// <summary>
        /// Returns 1 if up, -1 if down, 0 if flat
        /// </summary>
        public int Direction { get; set; }
    }
}




