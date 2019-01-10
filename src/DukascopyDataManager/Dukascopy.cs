using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using SevenZip.Compression.LZMA;
using DukascopyDataManager.Utilities;
using DukascopyDataManager.Interfaces.DataModels;
using DukascopyDataManager.Factories;

namespace DukascopyDataManager
{
    public class Dukascopy : IDisposable
    {
        private readonly WebClient _webClient;
        private readonly BigEndianConverter _bigEndianConverter;
        private readonly TickDataFactory _tickDataFactory;

        public Dukascopy()
        {
            _bigEndianConverter = new BigEndianConverter();
            _tickDataFactory = new TickDataFactory();
            _webClient = new WebClient();
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
        public byte[] DownloadData(string symbol, int year, int month, int day, int hour)
        {
            byte[] compressedData = _webClient.DownloadData($"http://www.dukascopy.com/datafeed/{symbol}/{year}/{month - 1:D2}/{day:D2}/{hour:D2}h_ticks.bi5");
            return Decompress(compressedData);
        }

        public List<ITickData> LoadTicksDirect(string symbol, int year, int month, int day, int hour)
        {
            DateTime dateTime = DateTime.Parse(string.Format("{0}/{1}/{2} {3}:00",
                year, month, day, hour));
            byte[] data = DownloadData(symbol, year, month, day, hour);

            return LoadTicks(data, dateTime);
        }

        /// <summary>
        /// Gets the ticks from the binary data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<ITickData> LoadTicks(byte[] data, DateTime dateTime)
        {
            var result = new List<ITickData>();

            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int milliseconds = _bigEndianConverter.GetInt32(reader.ReadBytes(4));
                    int ask = _bigEndianConverter.GetInt32(reader.ReadBytes(4));
                    int bid = _bigEndianConverter.GetInt32(reader.ReadBytes(4));
                    float askVolume = _bigEndianConverter.GetSingle(reader.ReadBytes(4));
                    float bidVolume = _bigEndianConverter.GetSingle(reader.ReadBytes(4));

                    result.Add(_tickDataFactory.GetNewTickData(dateTime, milliseconds,
                        bid, ask, bidVolume, askVolume));
                }
            }

            return result;
        }

        private static byte[] Decompress(byte[] inputBytes)
        {
            var newInStream = new MemoryStream(inputBytes);
            var decoder = new Decoder();

            newInStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
            {
                throw new Exception("input .lzma is too short");
            }

            long outSize = 0;
            for (int i = 0; i < 8; i++)
            {
                int v = newInStream.ReadByte();
                if (v < 0)
                {
                    throw new Exception("Can't Read 1");
                }

                outSize |= ((long)(byte)v) << (8 * i);
            }

            decoder.SetDecoderProperties(properties2);

            long compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            byte[] b = newOutStream.ToArray();
            return b;
        }

        #region IDisposable Support

        public void Dispose()
        {
            _webClient.Dispose();
        }

        #endregion IDisposable Support
    }
}