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

            var result = new byte[sizeof(int) + sizeof(byte) + facebookIdBytes.Length + sizeof(byte) + nameBytes.Length
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

            offset += ByteConverter.ReturnFromStream(data, offset, out byte facebookIdSize);
            offset += ByteConverter.ReturnFromStream(data, offset, facebookIdSize, out var facebookIdBytes);
            FacebookId = Encoding.UTF8.GetString(facebookIdBytes);

            offset += ByteConverter.ReturnFromStream(data, offset, out byte nameSize);
            offset += ByteConverter.ReturnFromStream(data, offset, nameSize, out var nameBytes);
            Name = Encoding.UTF8.GetString(nameBytes);

            offset += ByteConverter.ReturnFromStream(data, offset, out byte avatarPathSize);
            offset += ByteConverter.ReturnFromStream(data, offset, avatarPathSize, out var avatarPathBytes);
            AvatarPath = Encoding.UTF8.GetString(avatarPathBytes);
        }
    }
}
