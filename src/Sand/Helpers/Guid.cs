using Sand.Helpers.Internal;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Sand.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public enum SequentialGuidType
    {
        /// <summary>
        /// 
        /// </summary>
        SequentialAsString,
        /// <summary>
        /// 
        /// </summary>
        SequentialAsBinary,
        /// <summary>
        /// 
        /// </summary>
        SequentialAtEnd
    }
    /// <summary>
    /// 生产唯一识别号
    /// </summary>
    public static class Uuid
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        /// <summary>
        /// 生产唯一识别号
        /// </summary>
        /// <param name="guidType"></param>
        /// <returns></returns>
        public static string Next(SequentialGuidType guidType = SequentialGuidType.SequentialAsString)
        {
            byte[] randomBytes = new byte[10];
            _rng.GetBytes(randomBytes);
            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }
            byte[] guidBytes = new byte[16];
            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;

                case SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            return new Guid(guidBytes).ToString("N");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ObjectIdCreate()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}
