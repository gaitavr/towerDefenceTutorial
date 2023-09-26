
namespace Utils.Serialization
{
    public interface ISerializable
    {
        short GetLenght();
        void Serialize(byte[] data, ref int offset);
        void Deserialize(byte[] data, ref int offset);
    }
}
