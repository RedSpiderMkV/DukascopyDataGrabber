using DukascopyInstrumentManager.Enums;

namespace DukascopyInstrumentManager.Interfaces.DataModels
{
    /// <summary>
    /// Interface to the ticker instrument data object.
    /// </summary>
    public interface ITickerInstrument
    {
        /// <summary>
        /// Get the instrument name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the instrument symbol.
        /// </summary>
        string Symbol { get; }

        /// <summary>
        /// The type of instrument this object represents.
        /// </summary>
        InstrumentType Type { get; }
    }
}
