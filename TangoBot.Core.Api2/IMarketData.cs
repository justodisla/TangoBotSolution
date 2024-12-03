﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBot.Core.Api2
{
    /// <summary>
    /// Represents the historical plus current market data of a given symbol.
    /// IMarketData is an observable object. It notify observers of: DataChanged and CursorMoved
    /// In both cases it returns the affected DataPoint in the event.
    /// </summary>
    public interface IMarketData : IObservable<DataPoint>
    {
        /// <summary>
        /// Symbol of the market data
        /// </summary>
        string Symbol { get; }
        List<DataPoint> DataPoints { get; }
        /// <summary>
        /// The current dataPoint in the dataPoints list
        /// There is always a current DataPoint as if a cursor
        /// </summary>
        DataPoint Current { get; }
        /// <summary>
        /// Start date of the dataPoints list
        /// </summary>
        DateTime StartDate { get; }
        /// <summary>
        /// End date of the dataPoints list
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        /// Indicates where is the pointer in the dataPoints list (0 based)
        /// being 0 the first element of the list
        /// </summary>
        long CurrentIndex { get; }

        /// <summary>
        /// Returns the number of elements in the dataPoints list
        /// </summary>
        /// <returns></returns>
        long GetCount();

        /// <summary>
        /// Moves to the next element in the dataPoints list
        /// making it the current element
        /// </summary>
        void MoveNext();

        /// <summary>
        /// Moves the pointer to a given offset
        /// </summary>
        /// <param name="offSet"></param>
        void Move(int offSet);

        /// <summary>
        /// Returns the current element in the dataPoints list and moves the pointer to the next element
        /// by default it moves one element but it can be changed by passing a step parameter
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataPoint Step(int offset = 1);

        /// <summary>
        /// Moves the pointer to the first element in the dataPoints list
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataPoint StepToFirst(int offset = 0);

        /// <summary>
        /// Moves to the last element in the dataPoints list
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataPoint StepToLast(int offset = 0);
        /// <summary>
        /// Attaches indicators to the market data
        /// When indicators are attached, they are computed for each dataPoint
        /// in the dataPoints list
        /// </summary>
        /// <param name="indicatorName"></param>
        /// <param name="parameters"></param>
        void AttachIndicator(string indicatorName, KeyValuePair<string, double>[] parameters = null );

        /// <summary>
        /// Refreshes the indicators data based on the current dataPoints list
        /// </summary>
        void Compute();

        /// <summary>
        /// Resets the market data by loading the data from the server
        /// and reattaching the indicators
        /// and recomputing the indicators data into datapoints.
        /// </summary>
        void Reset();
    }
}
