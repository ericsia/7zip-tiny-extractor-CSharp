using pdj.tiny7z.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pdj.tiny7z.Archive
{
    public static class SevenZipMethods
    {
        public class MethodID : IEquatable<MethodID>
        {
            public MethodID() : this(new byte[0]) { }
            public MethodID(params byte[] id)
            {
                Raw = id.ToArray();
            }

            public int Size
            {
                get => Raw.Length;
            }

            public byte[] Raw
            {
                get;
            }

            public static bool operator ==(MethodID c1, MethodID c2)
            {
                if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                    return true;
                if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                    return false;
                return c1.Equals(c2);
            }

            public static bool operator !=(MethodID c1, MethodID c2)
            {
                return !(c1 == c2);
            }

            public override bool Equals(object obj)
            {
                return !ReferenceEquals(obj, null) && Equals(obj as MethodID);
            }

            public bool Equals(MethodID otherMethodID)
            {
                if (otherMethodID.Raw.Length != Raw.Length)
                    return false;
                return Raw.SequenceEqual(otherMethodID.Raw);
            }

            public override int GetHashCode()
            {
                return computeHash(Raw);
            }

            private static int computeHash(params byte[] data)
            {
                unchecked
                {
                    const int p = 16777619;
                    int hash = (int)2166136261;

                    for (int i = 0; i < data.Length; i++)
                        hash = (hash ^ data[i]) * p;

                    hash += hash << 13;
                    hash ^= hash >> 7;
                    hash += hash << 3;
                    hash ^= hash >> 17;
                    hash += hash << 5;
                    return hash;
                }
            }
        }

        public static readonly Dictionary<MethodID, string> List;
        public static readonly Dictionary<MethodID, Compression.Registry.Method> Supported;
        public static readonly Dictionary<Compression.Registry.Method, MethodID> Lookup;

        static SevenZipMethods()
        {
            try
            {
                List = new Dictionary<MethodID, string>
                {
                    { new MethodID( 0x21 ), "LZMA2" },
                    { new MethodID( 0x03, 0x01, 0x01 ), "7z LZMA" },
                    { new MethodID( 0x03, 0x03, 0x01, 0x1B ), "7z BCJ2 (4 packed streams)" },
                   
                };

                Supported = new Dictionary<MethodID, Compression.Registry.Method>
                {
                    { new MethodID( 0x21 ), Compression.Registry.Method.LZMA2 },
                    { new MethodID( 0x03, 0x01, 0x01 ), Compression.Registry.Method.LZMA },
                    { new MethodID( 0x03, 0x03, 0x01, 0x1B ), Compression.Registry.Method.BCJ2 },
                };

                Lookup = Supported.ToDictionary(kp => kp.Value, kp => kp.Key);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
