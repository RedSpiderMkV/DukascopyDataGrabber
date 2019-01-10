using DukascopyInstrumentManager.Enums;
using DukascopyInstrumentManager.Interfaces.DataModels;

namespace DukascopyInstrumentManager.Interfaces.Factories
{
    /// <summary>
    /// Interface to the TickerInstrumentFactory - used to create new TickerInstrument objects.
    /// </summary>
    public interface ITickerInstrumentFactory
    {
        /// <summary>
        /// Create a new ticker instrument object with the provided parameters.
        /// </summary>
        /// <param name="name">Instrument name.</param>
        /// <param name="symbol">Instrument symbol.</param>
        /// <param name="type">Instrument type.</param>
        /// <returns>New ticker instrument.</returns>
        ITickerInstrument GetNewTickerInstrument(string name, string symbol, InstrumentType type);
    }
}
