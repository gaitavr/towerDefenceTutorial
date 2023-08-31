using System.Text;
using Utils.Serialization;

namespace Core
{
    public sealed class UserSocialState : ISerializable
    {
        public int Version;
        public string FacebookId;
        public string Name;
        public string AvatarPath;

        public bool IsFacebook => string.IsNullOrWhiteSpace(FacebookId) == false;

        public byte[] Serialize()
        {
            var facebookIdBytes = Encoding.UTF8.GetBytes(FacebookId);
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            var avatarBytes = Encoding.UTF8.GetBytes(AvatarPath);

            var result = new byte[
                sizeof(int) 
                + sizeof(byte) + facebookIdBytes.Length 
                + sizeof(byte) + nameBytes.Length
                + sizeof(byte) + avatarBytes.Length];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);

            offset += ByteConverter.AddToStream((byte)facebookIdBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(facebookIdBytes, result, offset);

            offset += ByteConverter.AddToStream((byte)nameBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(nameBytes, result, offset);

            offset += ByteConverter.AddToStream((byte)avatarBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(avatarBytes, result, offset);

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);

            FacebookId = SerializationUtils.DeserealizeString(data, ref offset);
            Name = SerializationUtils.DeserealizeString(data, ref offset);
            AvatarPath = SerializationUtils.DeserealizeString(data, ref offset);
        }
    }
}
