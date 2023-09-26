using System;

namespace Utils.Serialization
{
    public static class ByteConverter
    {
        public static int AddToStream(byte source, byte[] destination, int offset)
        {
            destination[offset] = source;
            return 1;
        }
        
        public static int AddToStream(short source, byte[] destination, int offset)
        {
            destination[offset] = (byte)(source >> 8);
            destination[offset + 1] = (byte)source;
            return 2;
        }

        public static int AddToStream(int source, byte[] destination, int offset)
        {
            destination[offset] = (byte)(source >> 24);
            destination[offset + 1] = (byte)(source >> 16);
            destination[offset + 2] = (byte)(source >> 8);
            destination[offset + 3] = (byte)source;
            return 4;
        }

        //TODO check posibility to write directly to destination
        public static unsafe int AddToStream(float source, byte[] destination, int offset)
        {
            var bytes = new byte[4];
            fixed (byte* b = bytes)
                *((int*)b) = *(int*)&source;
            AddToStream(bytes, destination, offset);
            return 4;
        }

        public static int AddToStream(long source, byte[] destination, int offset)
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

        public static int ReturnFromStream(byte[] source, int offset, out byte destination)
        {
            destination = source[offset];
            return 1;
        }
        
        public static int ReturnFromStream(byte[] source, int offset, out short destination)
        {
            destination = (short)(source[offset] << 8);
            destination |= source[offset + 1];
            return 2;
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

        public static unsafe int ReturnFromStream(byte[] source, int offset, out float destination)
        {
            fixed (byte* ptr = &source[offset])
            {
                destination = * ((float*)(int*)ptr);
            }
            return 4;
        }

        public static int ReturnFromStream(byte[] source, int offset, out long destination)
        {
            destination = 0;
            destination |= (long)source[offset] << 56;
            destination |= (long)source[offset + 1] << 48;
            destination |= (long)source[offset + 2] << 40;
            destination |= (long)source[offset + 3] << 32;
            destination |= (long)source[offset + 4] << 24;
            destination |= (long)source[offset + 5] << 16;
            destination |= (long)source[offset + 6] << 8;
            destination |= (long)source[offset + 7];
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