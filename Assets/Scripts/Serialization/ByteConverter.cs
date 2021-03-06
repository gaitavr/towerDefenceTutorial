using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Serialization
{
    public static class ByteConverter
    {
        public static int AddToStream(byte source, byte[] destination, int offset)
        {
            destination[offset] = source;
            return 1;
        }

        public static int AddToStream(bool source, byte[] destination, int offset)
        {
            destination[offset] = (byte) (source ? 1 : 0);
            return 1;
        }

        public static int AddToStream(long source, byte[] destination, int offset)
        {
            destination[offset] = (byte) (source >> 56);
            destination[offset + 1] = (byte) (source >> 48);
            destination[offset + 2] = (byte) (source >> 40);
            destination[offset + 3] = (byte) (source >> 32);
            destination[offset + 4] = (byte) (source >> 24);
            destination[offset + 5] = (byte) (source >> 16);
            destination[offset + 6] = (byte) (source >> 8);
            destination[offset + 7] = (byte) (source);
            return 8;
        }

        public static int AddToStream(int source, byte[] destination, int offset)
        {
            destination[offset] = (byte) (source >> 24);
            destination[offset + 1] = (byte) (source >> 16);
            destination[offset + 2] = (byte) (source >> 8);
            destination[offset + 3] = (byte) source;
            return 4;
        }

        public static int AddToStream(float source, byte[] destination, int offset)
        {
            //Buffer.BlockCopy(BitConverter.GetBytes(source), 0, destination, offset, 4);
            FloatToByte(destination, offset, source);
            return 4;
        }

        public static int AddToStream(short source, byte[] destination, int offset)
        {
            destination[offset] = (byte) (source >> 8);
            destination[offset + 1] = (byte) (source);
            return 2;
        }

        public static int AddToStream(string str, byte[] destination, int offset)
        {
            byte[] data;
            if (string.IsNullOrEmpty(str))
            {
                data = new byte[0];
            }
            else
            {
                data = Encoding.UTF8.GetBytes(str);
            }

            System.Buffer.BlockCopy(data, 0, destination, offset, data.Length);
            return data.Length;
        }

        public static int AddToStream(double source, byte[] destination, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(source), 0, destination, offset, sizeof(double));
            return sizeof(double);
        }

        public static int AddToStream(byte[] source, byte[] destination, int offset)
        {
            Array.Copy(source, 0, destination, offset, source.Length);
            return source.Length;
        }

        public static int ReturnFromStream(byte[] source, int offset, out byte destination)
        {
            destination = source[offset];
            return 1;
        }

        public static int ReturnFromStream(byte[] source, int offset, out bool destination)
        {
            destination = source[offset] == 1;
            return 1;
        }

        public static int ReturnFromStream(byte[] source, int offset, out long destination)
        {
            destination = 0;
            destination |= (long) source[offset] << 56;
            destination |= (long) source[offset + 1] << 48;
            destination |= (long) source[offset + 2] << 40;
            destination |= (long) source[offset + 3] << 32;
            destination |= (long) source[offset + 4] << 24;
            destination |= (long) source[offset + 5] << 16;
            destination |= (long) source[offset + 6] << 8;
            destination |= (long) source[offset + 7];
            return 8;
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

        public static int ReturnFromStream(byte[] source, int offset, out short destination)
        {
            destination = 0;
            destination |= (short) (source[offset] << 8);
            destination |= (short) source[offset + 1];
            return 2;
        }

        public static int ReturnFromStream(byte[] source, int offset, out float destination)
        {
            destination = 0;
            destination = BitConverter.ToSingle(source, offset);
            return 4;
        }

        public static int ReturnFromStream(byte[] source, int offset, int lenght, out string destination)
        {
            destination = "";
            destination = Encoding.UTF8.GetString(source, offset, lenght);
            return lenght;
        }

        public static int ReturnFromStream(byte[] source, int offset, out double destination)
        {
            destination = BitConverter.ToDouble(source, offset);
            return sizeof(double);
        }

        public static int ReturnFromStream(byte[] source, int offset, int size, out byte[] destination)
        {
            destination = new byte[size];
            Array.Copy(source, offset, destination, 0, size);
            return size;
        }

        public static int GetByteCount(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return Encoding.UTF8.GetByteCount(str);
        }

        private static void FloatToByte(byte[] buffer, int offset, float value)
        {
            var tos = new ToSingle {Single = value};

            buffer[offset] = tos.Byte0;
            buffer[offset + 1] = tos.Byte1;
            buffer[offset + 2] = tos.Byte2;
            buffer[offset + 3] = tos.Byte3;
        }

        private static void ByteToFloat(byte[] buffer, int offset, out float value)
        {
            var tos = new ToSingle
            {
                Byte0 = buffer[offset],
                Byte1 = buffer[offset + 1],
                Byte2 = buffer[offset + 2],
                Byte3 = buffer[offset + 3],
            };

            value = tos.Single;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ToSingle
        {
            [FieldOffset(0)] public float Single;

            [FieldOffset(0)] public byte Byte0;

            [FieldOffset(1)] public byte Byte1;

            [FieldOffset(2)] public byte Byte2;

            [FieldOffset(3)] public byte Byte3;
        }
    }
}