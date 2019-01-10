
namespace DukascopyDataManager.Interfaces.Utilities
{
    /// <summary>
    /// Interface to the BigEndianConverter - converts a little endian byte array
    /// to a big endian value.
    /// </summary>
    public interface IBigEndianConverter
    {
        /// <summary>
        /// Get a 32 bit integer from the provided little endian byte array.
        /// </summary>
        /// <param name="byteArray">Little endian byte array.</param>
        /// <returns>Integer representation.</returns>
        int GetInt32(byte[] byteArray);

        /// <summary>
        /// Get a single precision float from the provided little endian byte array.
        /// </summary>
        /// <param name="byteArray">Little endian byte array.</param>
        /// <returns>Single precision float.</returns>
        float GetSingle(byte[] byteArray);
    }
}
