using System.Text;
using Utils.Serialization;

namespace Core
{
    public sealed class UserSocialState : ISerializable
    {
        public short Version;
        public string FacebookId;
        public string Name;
        public string AvatarPath;

        public bool IsFacebook => string.IsNullOrWhiteSpace(FacebookId) == false;

        public short GetLenght()
        {
            var lenght = sizeof(short) + SerializationUtils.GetSizeOfString(FacebookId) 
                + SerializationUtils.GetSizeOfString(Name) + SerializationUtils.GetSizeOfString(AvatarPath);
            return (short)lenght;
        }
        
        public void Serialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.AddToStream(Version, data, offset);
            SerializationUtils.SerializeString(FacebookId, data, ref offset);
            SerializationUtils.SerializeString(Name, data, ref offset);
            SerializationUtils.SerializeString(AvatarPath, data, ref offset);
        }

        public void Deserialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            FacebookId = SerializationUtils.DeserializeString(data, ref offset);
            Name = SerializationUtils.DeserializeString(data, ref offset);
            AvatarPath = SerializationUtils.DeserializeString(data, ref offset);
        }
    }
}
