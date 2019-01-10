using DukascopyInstrumentManager.DataModels;
using DukascopyInstrumentManager.Enums;
using DukascopyInstrumentManager.Interfaces.DataModels;
using DukascopyInstrumentManager.Interfaces.Factories;

namespace DukascopyInstrumentManager.Factories
{
    /// <summary>
    /// TickerInstrumentFactory - used to generate new ticker instrument objects.
    /// </summary>
    public class TickerInstrumentFactory : ITickerInstrumentFactory
    {
        /// <inheritdoc />
        public ITickerInstrument GetNewTickerInstrument(string name, string symbol, InstrumentType type)
        {
            return new TickerInstrument(name, symbol, type);
        }
    }
}
