using System.Collections.Generic;
using System.Text;

namespace Utils.Serialization
{
    public static class SerializationUtils
    {
        public static T Deserialize<T>(byte[] data, ref int offset) where T : ISerializable, new()
        {
            var result = new T();
            result.Deserialize(data, ref offset);
            return result;
        }
        
        public static short GetSizeOfList<T>(IReadOnlyList<T> sources) where T : ISerializable
        {
            var size = sizeof(short);
            foreach (var s in sources)
            {
                size += s.GetLenght();
            }
            return (short)size;
        }
        
        public static void SerializeList<T>(IReadOnlyList<T> sources, byte[] data, ref int offset) where T : ISerializable
        {
            offset += ByteConverter.AddToStream((short)sources.Count, data, offset);
            foreach (var s in sources)
            {
                s.Serialize(data, ref offset);
            }
        }
        
        public static List<T> DeserializeList<T>(byte[] data, ref int offset) where T : ISerializable, new()
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out short objectsCount);
            var result = new List<T>(objectsCount);
            for (var i = 0; i < objectsCount; i++)
            {
                var obj = Deserialize<T>(data, ref offset);
                result.Add(obj);
            }
            return result;
        }

        public static short GetSizeOfString(string source)
        {
            var lenght = sizeof(short) + source.Length * sizeof(char);
            return (short)lenght;
        }
        
        public static void SerializeString(string source, byte[] data, ref int offset)
        {
            var stringBytes = Encoding.UTF8.GetBytes(source);
            offset += ByteConverter.AddToStream((short)stringBytes.Length, data, offset);
            offset += ByteConverter.AddToStream(stringBytes, data, offset);
        }
        
        public static string DeserializeString(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out short stringSize);
            var result = Encoding.UTF8.GetString(data, offset, stringSize);
            offset += stringSize;
            return result;
        }
    }
}
