using System;

namespace DukascopyDataManager.Interfaces.DataModels
{
    /// <summary>
    /// Interface to TickData - used to represent trade information.
    /// </summary>
    public interface ITickData
    {
        /// <summary>
        /// Get tick data timestamp.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Get the bid price.
        /// </summary>
        int BidPrice { get; }

        /// <summary>
        /// Get the ask price.
        /// </summary>
        int AskPrice { get; }

        /// <summary>
        /// Get the bid volume (multiples of a million units).
        /// </summary>
        float BidVolume { get; }

        /// <summary>
        /// Get the ask volume (multiples of a million units).
        /// </summary>
        float AskVolume { get; }
    }
}
