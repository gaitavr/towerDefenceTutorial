using System;
using Utils.Serialization;

namespace Core
{
    public sealed class UserCurrenciesState : ISerializable
    {
        public int Version;
        public ulong Crystals;
        public ulong Gas;

        public byte[] Serialize()
        {
            var result = new byte[sizeof(int) + sizeof(ulong) + sizeof(ulong)];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream(Crystals, result, offset);
            offset += ByteConverter.AddToStream(Gas, result, offset);

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;
            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out Crystals);
            offset += ByteConverter.ReturnFromStream(data, offset, out Gas);
        }
    }
}
