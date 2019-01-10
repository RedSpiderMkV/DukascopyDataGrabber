using DukascopyInstrumentManager.Enums;
using DukascopyInstrumentManager.Interfaces.DataModels;

namespace DukascopyInstrumentManager.DataModels
{
    /// <summary>
    /// TickerInstrument - contains information relating to a ticker instrument.
    /// </summary>
    public class TickerInstrument : ITickerInstrument
    {
        #region Properties

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string Symbol { get; private set; }

        /// <inheritdoc />
        public InstrumentType Type { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Instantiate a new TickerInstrument object.
        /// </summary>
        /// <param name="name">Instrument name.</param>
        /// <param name="symbol">Instrument symbol.</param>
        /// <param name="type">Instument type.</param>
        public TickerInstrument(string name, string symbol, InstrumentType type)
        {
            Name = name;
            Symbol = symbol;
            Type = type;
        }

        #endregion
    }
}
