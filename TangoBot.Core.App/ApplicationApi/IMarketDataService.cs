using TangoBot.App.DTOs;
using TangoBot.Core.Domain.Aggregates;

namespace TangoBot.App.ApplicationApi
{
    /// <summary>
    /// Provides functionalities for market data operations.
    /// </summary>
    public interface IMarketDataService
    {
        // Queries

        /// <summary>
        /// Gets the market data for a specific symbol within a date range.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <param name="startDate">The start date of the date range.</param>
        /// <param name="endDate">The end date of the date range.</param>
        /// <returns>The market data for the specified symbol and date range.</returns>
        MarketDataDto GetMarketData(string symbol, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the market data for a specific symbol on a specific date.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <param name="date">The date for which to get the market data.</param>
        /// <returns>The market data for the specified symbol and date.</returns>
        MarketDataDto GetMarketData(string symbol, DateTime date);

        // Commands

        /// <summary>
        /// Subscribes to market data updates for a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to.</param>
        /// <param name="onUpdate">The action to perform when market data is updated.</param>
        void SubscribeToMarketData(string symbol, Action<MarketDataDto> onUpdate);

        /// <summary>
        /// Unsubscribes from market data updates for a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol to unsubscribe from.</param>
        void UnsubscribeFromMarketData(string symbol);

        // Static Historic Data

        /// <summary>
        /// Gets the historical market data for a specific symbol within a date range.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <param name="startDate">The start date of the date range.</param>
        /// <param name="endDate">The end date of the date range.</param>
        /// <returns>The historical market data for the specified symbol and date range.</returns>
        HistoricalDataDto GetHistoricalData(string symbol, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the historical market data for a specific symbol on a specific date.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <param name="date">The date for which to get the historical market data.</param>
        /// <returns>The historical market data for the specified symbol and date.</returns>
        HistoricalDataDto GetHistoricalData(string symbol, DateTime date);

        // Live Market Data Manager

        /// <summary>
        /// Creates a new instance of <see cref="LiveMarketDataManager"/>.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <param name="startDate">The start date of the market data.</param>
        /// <param name="endDate">The end date of the market data.</param>
        /// <param name="timeFrame">The time frame of the market data.</param>
        /// <returns>A new instance of <see cref="LiveMarketDataManager"/>.</returns>
        LiveMarketDataManager CreateLiveMarketDataManager(string symbol, DateTime startDate, DateTime endDate, TimeFrame timeFrame = TimeFrame.Day);

        /// <summary>
        /// Gets an existing instance of <see cref="LiveMarketDataManager"/> by symbol.
        /// </summary>
        /// <param name="symbol">The symbol of the market data.</param>
        /// <returns>An existing instance of <see cref="LiveMarketDataManager"/>.</returns>
        LiveMarketDataManager GetLiveMarketDataManager(string symbol);
    }
}
