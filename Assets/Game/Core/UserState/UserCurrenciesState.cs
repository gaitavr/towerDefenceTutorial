using System;
using Utils.Serialization;

namespace Core
{
    public sealed class UserCurrenciesState : ISerializable, IUserCurrenciesStateReadonly
    {
        public short Version;
        public int Crystals;
        public int Gas;
        public int Energy;

        public event Action Changed;

        int IUserCurrenciesStateReadonly.Crystals => Crystals;

        int IUserCurrenciesStateReadonly.Gas => Gas;
        int IUserCurrenciesStateReadonly.Energy => Energy;

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

        public void ChangeEnergy(int delta)
        {
            Energy += delta;
            Changed?.Invoke();
        }

        public short GetLenght()
        {
            var lenght = sizeof(short) + sizeof(int) + sizeof(int) + sizeof(int);
            return (short)lenght;
        }
        
        public void Serialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.AddToStream(Version, data, offset);
            offset += ByteConverter.AddToStream(Crystals, data, offset);
            offset += ByteConverter.AddToStream(Gas, data, offset);
            offset += ByteConverter.AddToStream(Energy, data, offset);
        }

        public void Deserialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out Crystals);
            offset += ByteConverter.ReturnFromStream(data, offset, out Gas);
            offset += ByteConverter.ReturnFromStream(data, offset, out Energy);
        }
    }
}
