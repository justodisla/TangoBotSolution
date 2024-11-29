using System;
using System.Threading.Tasks;

namespace TangoBot.API.Streaming
{
    /// <summary>
    /// Interface for streaming services that handle market data.
    /// </summary>
    /// <typeparam name="T">The type of data to be streamed.</typeparam>
    public interface IStreamingService
    {
        /// <summary>
        /// Streams historic data for a given symbol into the specified data object.
        /// </summary>
        /// <param name="symbol">The symbol to stream data for.</param>
        /// <param name="fromTime">The start time for the data stream.</param>
        /// <param name="toTime">The end time for the data stream.</param>
        /// <param name="timeframe">The timeframe for the data stream. Default is daily.</param>
        /// <param name="interval">The interval for the data stream. Default is 1.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the streamed data.</returns>
        void StreamHistoricDataAsync(string symbol, DateTime fromTime, DateTime toTime, Timeframe timeframe = Timeframe.Daily, int interval = 1);

        /// <summary>
        /// Patches the historic data into the specified data object.
        /// If the data object is not up to date, this method can be used to patch the missing data.
        /// For instance, if the data object has data up to 5 days ago, this method can be used to patch the data for the last 5 days up to the present.
        /// </summary>
        /// <param name="quoteDataHistory">The data object to patch.</param>
        void PatchHistoricData<T>(T quoteDataHistory);

        /// <summary>
        /// Streams live market data into the specified data object.
        /// The frequency of the streaming is determined by the data object.
        /// </summary>
        /// <param name="quoteDataHistory">The data object to stream live market data into.</param>
        void StreamLiveMarketData<T>(T quoteDataHistory);

        /// <summary>
        /// Validates the streaming authentication token.
        /// </summary>
        /// <param name="streamingToken">The streaming authentication token to validate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the token is valid.</returns>
        Task<bool> IsStreamingAuthTokenValid(string streamingToken);
    }

    /// <summary>
    /// Enum representing different timeframes for data streaming.
    /// </summary>
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
