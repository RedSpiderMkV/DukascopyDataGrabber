using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using DukascopyDataManager.Interfaces;
using DukascopyDataManager.Interfaces.DataModels;
using DukascopyDataManager.Interfaces.Factories;
using DukascopyDataManager.Interfaces.Utilities;
using SevenZip.Compression.LZMA;

namespace DukascopyDataManager
{
    /// <summary>
    /// Dukascopy data manager - retrieve finance data from Dukascopy.
    /// </summary>
    public class DukascopyManager : IDukascopyManager
    {
        #region Public Methods

        /// <summary>
        /// Instantiate a new object to download data from Dukascopy.
        /// </summary>
        /// <param name="bigEndianConverter">Converter to convert byte arrays from little endian to big endian.</param>
        /// <param name="tickDataFactory">Factory to generate new tick data objects.</param>
        /// <param name="webClient">Web client to access API.</param>
        public DukascopyManager(IBigEndianConverter bigEndianConverter, ITickDataFactory tickDataFactory, WebClient webClient)
        {
            _bigEndianConverter = bigEndianConverter;
            _tickDataFactory = tickDataFactory;
            _webClient = webClient;
        }

        /// <inheritdoc />
        public IEnumerable<ITickData> GetFullDayTickData(string symbol, DateTime retrievalTime)
        {
            var tickDataCollection = new List<ITickData>();

            for (int timeOffset = 0; timeOffset < 24; timeOffset++)
            {
                IEnumerable<ITickData> tickDataForHour = GetTickData(symbol, retrievalTime.Date.AddHours(timeOffset));
                tickDataCollection.AddRange(tickDataForHour);
            }

            return tickDataCollection;
        }

        /// <inheritdoc />
        public IEnumerable<ITickData> GetTickData(string symbol, DateTime retrievalTime)
        {
            var tickDataCollection = new List<ITickData>();

            byte[] dukaData = DownloadData(symbol, retrievalTime.Year, retrievalTime.Month,
                retrievalTime.Day, retrievalTime.Hour);

            if (dukaData == null)
            {
                return tickDataCollection;
            }

            try
            {
                tickDataCollection.AddRange(GenerateTickData(dukaData, retrievalTime));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return tickDataCollection;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate tick data from the byte array provided.
        /// </summary>
        /// <param name="dukaData">Duka data byte array.</param>
        /// <param name="retrievalTime">Retrieval time.</param>
        /// <returns>Collection of tick data.</returns>
        private IEnumerable<ITickData> GenerateTickData(byte[] dukaData, DateTime retrievalTime)
        {
            var tickDataCollection = new List<ITickData>();

            using (var reader = new BinaryReader(new MemoryStream(dukaData)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int milliseconds = _bigEndianConverter.GetInt32(reader.ReadBytes(4));
                    int ask = _bigEndianConverter.GetInt32(reader.ReadBytes(4));
                    int bid = _bigEndianConverter.GetInt32(reader.ReadBytes(4));
                    float askVolume = _bigEndianConverter.GetSingle(reader.ReadBytes(4));
                    float bidVolume = _bigEndianConverter.GetSingle(reader.ReadBytes(4));

                    tickDataCollection.Add(_tickDataFactory.GetNewTickData(retrievalTime,
                        milliseconds, bid, ask, bidVolume, askVolume));
                }
            }

            return tickDataCollection;
        }

        /// <summary>
        /// Downloads the tick data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>The binary data from the h_ticks.bi5 ticks file.</returns>
        private byte[] DownloadData(string symbol, int year, int month, int day, int hour)
        {
            byte[] decompressed = null;

            try
            {
                string url = $"http://www.dukascopy.com/datafeed/{symbol}/{year}/{month - 1:D2}/{day:D2}/{hour:D2}h_ticks.bi5";
                byte[] dukaData = _webClient.DownloadData(url);
                decompressed = Decompress(dukaData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return decompressed;
        }

        /// <summary>
        /// Decompress the LZMA compressed byte array.
        /// </summary>
        /// <param name="inputBytes">Byte array being decompressed.</param>
        /// <returns>Decompressed byte array.</returns>
        private static byte[] Decompress(byte[] inputBytes)
        {
            var newInStream = new MemoryStream(inputBytes);
            var decoder = new Decoder();

            newInStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
            {
                return null;
            }

            long outSize = 0;
            for (int i = 0; i < 8; i++)
            {
                int v = newInStream.ReadByte();
                if (v < 0)
                {
                    return null;
                }

                outSize |= ((long)(byte)v) << (8 * i);
            }

            decoder.SetDecoderProperties(properties2);

            long compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            byte[] b = newOutStream.ToArray();
            return b;
        }

        #endregion

        #region Private Data

        private readonly WebClient _webClient;
        private readonly IBigEndianConverter _bigEndianConverter;
        private readonly ITickDataFactory _tickDataFactory;

        #endregion
    }
}
