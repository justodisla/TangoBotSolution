﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.API.Persistence;

namespace TangoBot.API.Streaming
{
    /// <summary>
    /// QuoteDataHistory is a class that holds the historic and live market data for a given symbol.
    /// along with the timeframe and interval of the data, the symbol and the time range of the data.
    /// </summary>
    public class QuoteDataHistory 
    {
        public readonly List<DataPoint> DataPoints = new();

        public void AppendData(DataPoint dataPoint)
        {
            if (DataPoints.Any())
            {
                dataPoint.ForwardIndex = DataPoints.Last().ForwardIndex + 1;
            }
            else
            {
                dataPoint.ForwardIndex = 0;
            }

            DataPoints.Add(dataPoint);
        }

        public class DataPoint : AbstractEntity
        {
            public DataPoint(decimal open, decimal high, decimal low, decimal close, DateTime time, double volume, double vwap, double bidVolume, double askVolume, double impVolatility)
            {
                Open = open;
                High = high;
                Low = low;
                Close = close;
                Time = time;
                Volume = volume;
                Vwap = vwap;
                BidVolume = bidVolume;
                AskVolume = askVolume;
                ImpVolatility = impVolatility;
            }

            public decimal Open { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Close { get; set; }
            public DateTime Time { get; set; }
            public double Volume { get; set; }
            public double Vwap { get; set; }
            public double BidVolume { get; set; }
            public double AskVolume { get; set; }
            public double ImpVolatility { get; set; }
            internal double ForwardIndex { get; set; }

            public void AfterSave()
            {
                //throw new NotImplementedException();
            }

            public void BeforeSave()
            {
                //throw new NotImplementedException();
            }

            public override string GetDescription()
            {
                return "";
            }

            public override string GetEntityName()
            {
               return "DataPoint";
            }

            public override string GetTableName()
            {
                return "DataPoints";
            }

            public override string ToString()
            {
                return $"Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Time: {Time}, Volume: {Volume}, Vwap: {Vwap}, BidVolume: {BidVolume}, AskVolume: {AskVolume}, ImpVolatility: {ImpVolatility}";
            }

            public bool Validate()
            {
                return true;
            }
        }
    }

}
