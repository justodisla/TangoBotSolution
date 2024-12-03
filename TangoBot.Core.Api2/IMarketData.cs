using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBot.Core.Api2
{
    /// <summary>
    /// Represents the historical plus current market data of a given symbol.
    /// 
    /// </summary>
    public interface IMarketData
    {
        string Symbol { get; }
        List<DataPoint> dataPoints { get; }

        DataPoint Current { get; }

        DateTime StartDate { get; }
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

        void AttachIndicator(string indicatorName, KeyValuePair<string, double>[] parameters = null );
    
        void Compute();
    }
}
