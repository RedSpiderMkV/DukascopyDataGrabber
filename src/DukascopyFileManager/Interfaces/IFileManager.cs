using System;
using System.Collections.Generic;
using DukascopyDataManager.Interfaces.DataModels;

namespace DukascopyFileManager.Interfaces
{
    /// <summary>
    /// Interface to the FileManager - used to handle dukascopy data files.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Save the provided data collection as a CSV file.
        /// </summary>
        /// <param name="InstrumentName">Name of instrument.</param>
        /// <param name="timestamp">Timestamp of data being downloaded.</param>
        /// <param name="dataCollection">Data collection to be saved.</param>
        void SaveData(string InstrumentName, DateTime timestamp, IEnumerable<ITickData> dataCollection);
    }
}
