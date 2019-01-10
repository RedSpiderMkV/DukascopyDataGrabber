using System;
using DukascopyDataManager.Interfaces.DataModels;

namespace DukascopyDataManager.Interfaces.Factories
{
    /// <summary>
    /// Interface to the TickDataFactory - used to generate TickData objects.
    /// </summary>
    public interface ITickDataFactory
    {
        /// <summary>
        /// Get new tick data object from the parameters provided.
        /// </summary>
        /// <param name="baseDateTime">Base time stamp.</param>
        /// <param name="milliseconds">Offset from base time stamp in milliseconds.</param>
        /// <param name="bid">Bid price.</param>
        /// <param name="ask">Ask price.</param>
        /// <param name="bidVolume">Bid volume.</param>
        /// <param name="askVolume">Ask volume.</param>
        /// <returns>Instantiated tick data object.</returns>
        ITickData GetNewTickData(DateTime baseDateTime, int milliseconds, int bid, int ask, float bidVolume, float askVolume);
    }
}
