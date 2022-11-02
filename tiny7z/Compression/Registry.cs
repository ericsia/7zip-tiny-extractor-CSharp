using System;
using System.IO;
using System.Linq;

namespace pdj.tiny7z.Compression
{
    public static class Registry
    {
        /// <summary>
        /// List of supported coding/decoding methods.
        /// </summary>
        public enum Method
        {
            BCJ2,
            LZMA,
            LZMA2,
        }

        /// <summary>
        /// Creates a stream of a specific decoder type.
        /// </summary>
        public static Stream GetDecoderStream(
            Method method,
            Stream[] inStreams,
            Byte[] properties,
            IPasswordProvider password,
            long limit)
        {
            switch (method)
            {
             
               
              // these 3 is a must for lzma2
                case Method.BCJ2:
                    return new Bcj2DecoderStream(inStreams, properties, limit);
                case Method.LZMA:
                    return new LzmaDecoderStream(inStreams.Single(), properties, limit);
                case Method.LZMA2:
                    return new Lzma2DecoderStream(inStreams.Single(), properties.First(), limit);
              
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
