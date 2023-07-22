using System;

public static class ByteConverter
{
    public static int AddToStream(byte source, byte[] destination, int offset)
    {
        destination[offset] = source;
        return 1;
    }

    public static int AddToStream(int source, byte[] destination, int offset)
    {
        destination[offset] = (byte) (source >> 24);
        destination[offset + 1] = (byte) (source >> 16);
        destination[offset + 2] = (byte) (source >> 8);
        destination[offset + 3] = (byte) source;
        return 4;
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

    public static int ReturnFromStream(byte[] source, int offset, int size, out byte[] destination)
    {
        destination = new byte[size];
        Array.Copy(source, offset, destination, 0, size);
        return size;
    }
}