using System;
using DukascopyDataManager.Interfaces.DataModels;

namespace DukascopyDataManager.DataModels
{
    /// <summary>
    /// TickData - represents trade information.
    /// </summary>
    internal class TickData : ITickData
    {
        #region Properties

        /// <inheritdoc />
        public int AskPrice { get; private set; }

        /// <inheritdoc />
        public int BidPrice { get; private set; }

        /// <inheritdoc />
        public float AskVolume { get; private set; }

        /// <inheritdoc />
        public float BidVolume { get; private set; }

        /// <inheritdoc />
        public DateTime Timestamp { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Instantiate a new TickData object with the provided parameters.
        /// </summary>
        /// <param name="baseDateTime">Base time stamp.</param>
        /// <param name="milliseconds">Offset from base time stamp in milliseconds.</param>
        /// <param name="bid">Bid price.</param>
        /// <param name="ask">Ask price.</param>
        /// <param name="bidVolume">Bid volume.</param>
        /// <param name="askVolume">Ask volume.</param>
        public TickData(DateTime baseDateTime, int milliseconds, int bid, int ask, float bidVolume, float askVolume)
        {
            Timestamp = baseDateTime.AddMilliseconds(milliseconds);
            AskPrice = ask;
            BidPrice = bid;
            AskVolume = askVolume;
            BidVolume = bidVolume;
        }

        #endregion
    }
}
