using System;
using DukascopyDataManager.DataModels;
using DukascopyDataManager.Interfaces.DataModels;
using DukascopyDataManager.Interfaces.Factories;

namespace DukascopyDataManager.Factories
{
    /// <summary>
    /// TickDataFactory - used to generate new tick data objects.
    /// </summary>
    public class TickDataFactory : ITickDataFactory
    {
        /// <inheritdoc />
        public ITickData GetNewTickData(DateTime baseDateTime, int milliseconds, int bid,
            int ask, float bidVolume, float askVolume)
        {
            return new TickData(baseDateTime, milliseconds, bid, ask, bidVolume, askVolume);
        }
    }
}
