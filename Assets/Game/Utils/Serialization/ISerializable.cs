

namespace Utils.Serialization
{
    public interface ISerializable
    {
        byte[] Serialize();
        void Deserialize(byte[] data);
    }
}
