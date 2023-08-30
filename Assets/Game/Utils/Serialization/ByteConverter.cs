using System;
using System.Collections.Generic;

namespace Utils.Serialization
{
    public static class ByteConverter
    {
        public static int AddToStream(byte source, byte[] destination, int offset)
        {
            destination[offset] = source;
            return 1;
        }

        public static int AddToStream(int source, byte[] destination, int offset)
        {
            destination[offset] = (byte)(source >> 24);
            destination[offset + 1] = (byte)(source >> 16);
            destination[offset + 2] = (byte)(source >> 8);
            destination[offset + 3] = (byte)source;
            return 4;
        }

        public static byte[] Serialize(int source)
        {
            var result = new byte[4];
            AddToStream(source, result, 0);
            return result;
        }

        public static int AddToStream(ulong source, byte[] destination, int offset)
        {
            destination[offset] = (byte)(source >> 56);
            destination[offset + 1] = (byte)(source >> 48);
            destination[offset + 2] = (byte)(source >> 40);
            destination[offset + 3] = (byte)(source >> 32);
            destination[offset + 4] = (byte)(source >> 24);
            destination[offset + 5] = (byte)(source >> 16);
            destination[offset + 6] = (byte)(source >> 8);
            destination[offset + 7] = (byte)source;
            return 8;
        }

        public static int AddToStream(byte[] source, byte[] destination, int offset)
        {
            for (var i = 0; i < source.Length; i++)
            {
                destination[offset + i] = source[i];
            }
            return source.Length;
        }

        public static int AddToStream(List<byte> source, byte[] destination, int offset)
        {
            for (var i = 0; i < source.Count; i++)
            {
                destination[offset + i] = source[i];
            }
            return source.Count;
        }

        public static int ReturnFromStream(byte[] source, int offset, out byte destination)
        {
            destination = source[offset];
            return 1;
        }

        public static int ReturnFromStream(byte[] source, int offset, out int destination)
        {
            destination = 0;
            destination |= source[offset] << 24;
            destination |= source[offset + 1] << 16;
            destination |= source[offset + 2] << 8;
            destination |= source[offset + 3];
            return 4;
        }

        public static int ReturnFromStream(byte[] source, int offset, out ulong destination)
        {
            destination = 0;
            destination |= (ulong)source[offset] << 56;
            destination |= (ulong)source[offset + 1] << 48;
            destination |= (ulong)source[offset + 2] << 40;
            destination |= (ulong)source[offset + 3] << 32;
            destination |= (ulong)source[offset + 4] << 24;
            destination |= (ulong)source[offset + 5] << 16;
            destination |= (ulong)source[offset + 6] << 8;
            destination |= (ulong)source[offset + 7];
            return 8;
        }

        public static int ReturnFromStream(byte[] source, int offset, int size, out byte[] destination)
        {
            destination = new byte[size];
            Array.Copy(source, offset, destination, 0, size);
            return size;
        }
    }
}