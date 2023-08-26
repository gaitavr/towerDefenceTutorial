using System;
using Utils.Serialization;

namespace Core
{
    public sealed class UserCurrenciesState : ISerializable, IUserCurrenciesStateReadonly
    {
        public int Version;
        public int Crystals;
        public int Gas;

        public event Action Changed;

        int IUserCurrenciesStateReadonly.Crystals => Crystals;

        int IUserCurrenciesStateReadonly.Gas => Gas;

        public void ChangeCrystals(int delta)
        {
            Crystals += delta;
            Changed?.Invoke();
        }

        public void ChangeGas(int delta)
        {
            Gas += delta;
            Changed?.Invoke();
        }

        public byte[] Serialize()
        {
            var result = new byte[sizeof(int) + sizeof(int) + sizeof(int)];

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
