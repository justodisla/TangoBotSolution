using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Streaming
{
    public interface IStreamService<T>
    {
        /// <summary>
        /// Stream historic data for a given symbol into the QuoteDataHistory object.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="timeframe"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        //QuoteDataHistory StreamHistoricData(string symbol, DateTime fromTime, DateTime toTime, Timeframe timeframe = Timeframe.Daily, int interval = 1);

        public  Task<T> StreamHistoricDataAsync(string symbol, DateTime fromTime, DateTime toTime, Timeframe timeframe = Timeframe.Daily, int interval = 1);


        /// <summary>
        /// Patch the historic data into the QuoteDataHistory object.
        /// If the QuoteDataHistory is not up to date, this method can be used to patch the missing data.
        /// For instance if QuoteDataHistory has data up to 5 days ago, this method can be used to patch the data for the last 5 days up to present
        /// </summary>
        /// <param name="quoteDataHistory"></param>
        void PatchHistoricData(T quoteDataHistory);



        /// <summary>
        /// Stream live market data into the QuoteDataHistory object.
        /// The frequency of the streaming is determined by the QuoteDataHistory object.
        /// </summary>
        /// <param name=""></param>
        void StreamLiveMarketData(T quoteDataHistory);

        /// <summary>
        /// Closes the WebSocket connection.
        /// </summary>
        void CloseWsConnection();
        Task<bool> IsStreamingAuthTokenValid();
    }

    public enum Timeframe
    {
        OneHour,
        FourHour,
        Daily,
        Weekly,
        Monthly,
        Year,

    }
}
