using System.Collections.Generic;
using DukascopyInstrumentManager.Interfaces.DataModels;

namespace DukascopyInstrumentManager.Interfaces
{
    /// <summary>
    /// Interface to the InstrumentManager - used to retrieve instrument information.
    /// </summary>
    public interface IInstrumentManager
    {
        /// <summary>
        /// Retrieve a collection of ticker instruments to be downloaded.
        /// </summary>
        /// <returns>Collection of ticker instruments.</returns>
        IEnumerable<ITickerInstrument> GetTickerInstruments();
    }
}
