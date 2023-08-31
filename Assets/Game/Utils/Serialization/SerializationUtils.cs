using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Serialization
{
    public static class SerializationUtils
    {
        public static List<byte> SerializeList<T>(IEnumerable<T> sources) where T : ISerializable
        {
            var count = sources.Count();
            var result = new List<byte>(count * 2048)
            {
                (byte)count
            };
            foreach (var source in sources)
            {
                var serializedSource = source.Serialize();
                result.AddRange(ByteConverter.Serialize(serializedSource.Length));
                result.AddRange(serializedSource);
            }
            return result;
        }

        public static T Deserialize<T>(byte[] data, ref int offset) where T : ISerializable, new()
        {
            var result = new T();
            offset += ByteConverter.ReturnFromStream(data, offset, out int objectSize);
            offset += ByteConverter.ReturnFromStream(data, offset, objectSize, out var bytesToRead);
            result.Deserialize(bytesToRead);
            return result;
        }

        public static List<T> DeserializeList<T>(byte[] data, ref int offset) where T : ISerializable, new()
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out byte objectsCount);
            var result = new List<T>(objectsCount);
            for (int i = 0; i < objectsCount; i++)
            {
                var obj = Deserialize<T>(data, ref offset);
                result.Add(obj);
            }
            return result;
        }

        public static string DeserealizeString(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out byte stringSize);
            offset += ByteConverter.ReturnFromStream(data, offset, stringSize, out var bytesToRead);
            return Encoding.UTF8.GetString(bytesToRead);
        }
    }
}
