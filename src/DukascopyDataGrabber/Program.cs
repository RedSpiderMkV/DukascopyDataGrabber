using System;
using System.Collections.Generic;
using System.Net;
using DukascopyDataManager;
using DukascopyDataManager.Factories;
using DukascopyDataManager.Interfaces;
using DukascopyDataManager.Interfaces.DataModels;
using DukascopyDataManager.Utilities;
using DukascopyFileManager;
using DukascopyFileManager.Interfaces;
using DukascopyInstrumentManager;
using DukascopyInstrumentManager.Factories;
using DukascopyInstrumentManager.Interfaces;
using DukascopyInstrumentManager.Interfaces.DataModels;

namespace DukascopyDataGrabber
{
    internal class Program
    {
        private static IInstrumentManager symbolManager;
        private static IDukascopyManager duka;
        private static IFileManager fileManager;

        internal static void Main(string[] args)
        {
            DateTime startDate, endDate;
            if (args.Length != 2 || !DateTime.TryParse(args[0], out startDate) || !DateTime.TryParse(args[1], out endDate))
            {
                InvalidArgumentMessage();
                Console.ReadKey();
                return;
            }

            symbolManager = GetSymbolManager();
            duka = GetDukascopyManager();
            fileManager = GetFileManager();

            IEnumerable<ITickerInstrument> symbolsCollection = symbolManager.GetTickerInstruments();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            //while (startDate <= endDate)
            //{
            //    Console.WriteLine("Date: " + startDate.ToShortDateString());
            //    foreach (ITickerInstrument tickerSymbol in symbolsCollection)
            //    {
            //        Console.WriteLine("Processing: " + tickerSymbol.Name);

            //        IEnumerable<ITickData> tickDataCollection = duka.GetFullDayTickData(tickerSymbol.Symbol, startDate);
            //        fileManager.SaveData(tickerSymbol.Name, startDate, tickDataCollection);

            //        //PrintDataToConsole(tickerSymbol.Symbol, duka, yesterday);
            //        //break;
            //    }

            //    startDate = startDate.AddDays(1);
            //}
            // 2017-04-18 to 2017-04-20: elapsed: 976189 ms

            //foreach (ITickerInstrument tickerSymbol in symbolsCollection)
            //{
            //    startDate = DateTime.Parse(args[0]);
            //    Console.WriteLine("Processing: " + tickerSymbol.Name);

            //    while (startDate <= endDate)
            //    {
            //        Console.WriteLine("Date: " + startDate.ToShortDateString());

            //        IEnumerable<ITickData> tickDataCollection = duka.GetFullDayTickData(tickerSymbol.Symbol, startDate);
            //        fileManager.SaveData(tickerSymbol.Name, startDate, tickDataCollection);

            //        //PrintDataToConsole(tickerSymbol.Symbol, duka, yesterday);
            //        //break;

            //        startDate = startDate.AddDays(1);
            //    }
            //}
            // 2017-04-18 to 2017-04-20: elapsed: 989981 ms

            foreach (ITickerInstrument tickerSymbol in symbolsCollection)
            {
                DownloadData(startDate, endDate, tickerSymbol.Symbol, tickerSymbol.Name);
            }
            // 2017-04-18 to 2017-04-20: non threadpool - elapsed: 798629 ms
            // 2017-04-18 to 2017-04-20: via WriteAllLines - elapsed: 145391 ms!!! :)

            watch.Stop();
            Console.WriteLine("EXECUTION: " + watch.ElapsedMilliseconds);
            ExitMessage();
        }

        private static void DownloadData(DateTime startDate, DateTime endDate, string symbol, string name)
        {
            Console.WriteLine("Processing: " + name);

            var tickDataCollection = new List<ITickData>();

            while (startDate <= endDate)
            {
                Console.WriteLine("Date: " + startDate.ToShortDateString());
                tickDataCollection.AddRange(duka.GetFullDayTickData(symbol, startDate));

                startDate = startDate.AddDays(1);
            }

            Console.WriteLine("Writing " + symbol);
            fileManager.SaveData(name, endDate, tickDataCollection);
        }

        private static void InvalidArgumentMessage()
        {
            Console.WriteLine("Invalid argument.");
            Console.WriteLine("Usage: DukascopyDataGrabber START(yyyy-mm-dd) END(yyyy-mm-dd)");
            Console.WriteLine("");

            ExitMessage();
        }

        private static void ExitMessage()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void PrintDataToConsole(string symbol, IDukascopyManager duka, DateTime date)
        {
            IEnumerable<ITickData> tickDataCollection = duka.GetFullDayTickData(symbol, date);

            foreach (ITickData tickData in tickDataCollection)
            {
                Console.WriteLine(tickData.Timestamp.ToShortDateString() + " " + tickData.Timestamp.Hour + ":" + tickData.Timestamp.Minute
                    + ":" + tickData.Timestamp.Second + "." + tickData.Timestamp.Millisecond.ToString().PadLeft(3, '0')
                    + "\t\t" + tickData.BidPrice + "\t" + tickData.AskPrice + "\t" + tickData.BidVolume + "\t" + tickData.AskVolume);
            }
        }

        private static IFileManager GetFileManager()
        {
            var fileManager = new FileManager();

            return fileManager;
        }

        private static IInstrumentManager GetSymbolManager()
        {
            var symbolFactory = new TickerInstrumentFactory();
            IInstrumentManager symbolManager = new InstrumentManager(symbolFactory);

            return symbolManager;
        }

        private static IDukascopyManager GetDukascopyManager()
        {
            var bigEndianConverter = new BigEndianConverter();
            var tickDataFactory = new TickDataFactory();
            var webClient = new WebClient();
            var duka = new DukascopyManager(bigEndianConverter, tickDataFactory, webClient);

            return duka;
        }
    }
}
