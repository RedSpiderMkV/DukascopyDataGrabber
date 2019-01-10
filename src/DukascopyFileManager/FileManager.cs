using System;
using System.Collections.Generic;
using System.IO;
using DukascopyDataManager.Interfaces.DataModels;
using DukascopyFileManager.Interfaces;

namespace DukascopyFileManager
{
    /// <summary>
    /// FileManager - used to handle dukascopy data files.
    /// </summary>
    public class FileManager : IFileManager
    {
        #region Public Methods

        /// <summary>
        /// Instantiate a new FileManager to handle file operations.
        /// </summary>
        public FileManager()
        {
            CreateDirectoryIfNotExists(DOWNLOAD_DIRECTORY);
        }

        /// <inheritdoc />
        public void SaveData(string InstrumentName, DateTime timestamp, IEnumerable<ITickData> dataCollection)
        {
            string directoryTimestamp = $"{timestamp.Year}{timestamp.Month.ToString().PadLeft(2, '0')}";
            string subDirectoryPath = Path.Combine(DOWNLOAD_DIRECTORY, directoryTimestamp);
            CreateDirectoryIfNotExists(subDirectoryPath);

            string fileName = Path.Combine(subDirectoryPath, $"{InstrumentName}_{directoryTimestamp}.csv");
            CreateFileIfNotExists(fileName);

            var fileContents = new List<string>();
            foreach(ITickData data in dataCollection)
            {
                string dataTimestamp = data.Timestamp.ToShortDateString() + " " + data.Timestamp.Hour.ToString().PadLeft(2, '0')
                    + ":" + data.Timestamp.Minute.ToString().PadLeft(2, '0') + ":" + data.Timestamp.Second.ToString().PadLeft(2, '0')
                    + "." + data.Timestamp.Millisecond.ToString().PadLeft(3, '0');

                string content = $"{dataTimestamp},{data.BidPrice},{data.AskPrice},{data.BidVolume},{data.AskVolume}";
                fileContents.Add(content);
            }

            File.AppendAllLines(fileName, fileContents);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a directory corresponding to the provided path if it doesn't exist.
        /// </summary>
        /// <param name="directoryPath">Directory path.</param>
        private static void CreateDirectoryIfNotExists(string directoryPath)
        {
            if(Directory.Exists(directoryPath))
            {
                return;
            }

            Directory.CreateDirectory(directoryPath);
        }

        /// <summary>
        /// Create a new file for csv data if it doesn't exist already.
        /// </summary>
        /// <param name="fileName">Name of file.</param>
        private static void CreateFileIfNotExists(string fileName)
        {
            if(File.Exists(fileName))
            {
                return;
            }

            File.Create(fileName).Dispose();
            File.AppendAllText(fileName, "TimeStamp,Bid,Ask,BidVolume,AskVolume\n");
        }

        #endregion

        #region Private Data

        private const string DOWNLOAD_DIRECTORY = "Data";

        #endregion
    }
}
