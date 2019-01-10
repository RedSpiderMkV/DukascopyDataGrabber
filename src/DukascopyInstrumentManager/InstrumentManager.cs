using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DukascopyInstrumentManager.Enums;
using DukascopyInstrumentManager.Interfaces;
using DukascopyInstrumentManager.Interfaces.DataModels;
using DukascopyInstrumentManager.Interfaces.Factories;

namespace DukascopyInstrumentManager
{
    /// <summary>
    /// Instrument manager - retrieves instrument symbols to be downloaded.
    /// </summary>
    public class InstrumentManager : IInstrumentManager
    {
        #region Public Methods

        /// <summary>
        /// Instantiate a new InstrumentManager to retrieve instrument symbols to be downloaded.
        /// </summary>
        /// <param name="tickerInstrumentFactory">Ticker symbol factory to generate ticker symbol objects.</param>
        public InstrumentManager(ITickerInstrumentFactory tickerInstrumentFactory)
        {
            if (tickerInstrumentFactory == null)
            {
                throw new NullReferenceException("SymbolManager: tickerSymbolFactory cannot be null.");
            }

            _tickerInstrumentFactory = tickerInstrumentFactory;
            _instrumentsXml = XElement.Load(INSTRUMENT_FILE);
        }

        /// <inheritdoc />
        public IEnumerable<ITickerInstrument> GetTickerInstruments()
        {
            IEnumerable<ITickerInstrument> equities = GetTickerInstrumentsFromXml(InstrumentType.Equity);
            IEnumerable<ITickerInstrument> commodities = GetTickerInstrumentsFromXml(InstrumentType.Commodity);
            IEnumerable<ITickerInstrument> forex = GetTickerInstrumentsFromXml(InstrumentType.Forex);
            IEnumerable<ITickerInstrument> indices = GetTickerInstrumentsFromXml(InstrumentType.Index);

            var tickerInstrumentsCollection = new List<ITickerInstrument>();
            tickerInstrumentsCollection.AddRange(equities);
            tickerInstrumentsCollection.AddRange(commodities);
            tickerInstrumentsCollection.AddRange(forex);
            tickerInstrumentsCollection.AddRange(indices);

            return tickerInstrumentsCollection;
        }

        #endregion

        /// <summary>
        /// Retrieve a collection of ticker instruments from the loaded xml.
        /// </summary>
        /// <param name="type">Type of instrument to be retrieved.</param>
        /// <returns>Collection of ticker instruments.</returns>
        private IEnumerable<ITickerInstrument> GetTickerInstrumentsFromXml(InstrumentType type)
        {
            string instrumentName = GetElementName(type);
            XElement element = _instrumentsXml.Element(instrumentName);

            var tickerInstrumentCollection = new List<ITickerInstrument>();

            IEnumerable<XElement> indices = element.Elements("Instrument");
            foreach (XElement index in indices)
            {
                string name = index.Attribute("Name")?.Value;
                string symbol = index.Attribute("Symbol")?.Value;

                ITickerInstrument tickerInstrument = _tickerInstrumentFactory.GetNewTickerInstrument(name, symbol, InstrumentType.Forex);
                tickerInstrumentCollection.Add(tickerInstrument);
            }

            return tickerInstrumentCollection;
        }

        /// <summary>
        /// Get the element name from the instrument type provided.
        /// </summary>
        /// <param name="type">Instrument type.</param>
        /// <returns>Name of XML element corresponding to provided type.</returns>
        private static string GetElementName(InstrumentType type)
        {
            switch (type)
            {
                case InstrumentType.Commodity:
                    return "Commodities";
                case InstrumentType.Forex:
                    return "Forex";
                case InstrumentType.Index:
                    return "Indices";
                case InstrumentType.Equity:
                    return "Equities";
                case InstrumentType.Bond:
                    return "Bonds";
                case InstrumentType.Crypto:
                    return "Cryptocurrencies";
                default:
                    return null;
            }
        }

        #region Private Data

        private readonly XElement _instrumentsXml;
        private readonly ITickerInstrumentFactory _tickerInstrumentFactory;

        private const string INSTRUMENT_FILE = "DownloadSymbols.xml";

        #endregion
    }
}
