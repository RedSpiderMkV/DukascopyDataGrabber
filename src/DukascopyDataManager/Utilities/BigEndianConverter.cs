using System;
using DukascopyDataManager.Interfaces.Utilities;

namespace DukascopyDataManager.Utilities
{
    /// <summary>
    /// BigEndianConverter - converts little endian byte arrays to big endian values.
    /// </summary>
    public class BigEndianConverter : IBigEndianConverter
    {
        /// <inheritdoc />
        public int GetInt32(byte[] byteArray)
        {
            if(byteArray.Length != 4)
            {
                return -1;
            }

            Array.Reverse(byteArray);

            int intValue = BitConverter.ToInt32(byteArray, 0);
            return intValue;
        }

        /// <inheritdoc />
        public float GetSingle(byte[] byteArray)
        {
            if(byteArray.Length != 4)
            {
                return -1;
            }

            Array.Reverse(byteArray);

            float singleValue = BitConverter.ToSingle(byteArray, 0);
            return singleValue;
        }
    }
}
