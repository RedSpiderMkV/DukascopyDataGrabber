using System;
using System.Collections.Generic;
using DukascopyDataManager.Interfaces.DataModels;

namespace DukascopyDataManager.Interfaces
{
    /// <summary>
    /// Interface to the Dukascopy data manager - used to retrieve finance data.
    /// </summary>
    public interface IDukascopyManager
    {
        /// <summary>
        /// Downloads the tick data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>Collection of tick data.</returns>
        IEnumerable<ITickData> GetTickData(string symbol, DateTime retrievalTime);

        /// <summary>
        /// Download a full day worth of data for the provided symbol.
        /// </summary>
        /// <param name="symbol">Symbol who's data is to be downloaded.</param>
        /// <param name="retrievalTime">Date of data to be retrieved.</param>
        /// <returns>Collection of tick data.</returns>
        IEnumerable<ITickData> GetFullDayTickData(string symbol, DateTime retrievalTime);
    }
}
