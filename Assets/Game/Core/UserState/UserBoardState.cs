using UnityEngine;
using Utils.Serialization;

namespace Core
{
    public sealed class UserBoardState : ISerializable
    {
        public short Version;
        public string Name;
        public byte X;
        public byte Y;
        public GameTileContentType[] Content;
        public byte[] Levels;
        public bool Selected;

        public short GetLenght()
        {
            var lenght = sizeof(short)
                + SerializationUtils.GetSizeOfString(Name)
                + sizeof(byte) + sizeof(byte)
                + sizeof(byte) * Content.Length 
                + sizeof(byte) * Levels.Length
                + sizeof(byte);
            
            return (short)lenght;
        }
        
        public void Serialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.AddToStream(Version, data, offset);
            SerializationUtils.SerializeString(Name, data, ref offset);
            offset += ByteConverter.AddToStream(X, data, offset);
            offset += ByteConverter.AddToStream(Y, data, offset);

            foreach (var c in Content)
            {
                offset += ByteConverter.AddToStream((byte)c, data, offset);
            }

            foreach (var l in Levels)
            {
                offset += ByteConverter.AddToStream(l, data, offset);
            }

            offset += ByteConverter.AddToStream(Selected, data, offset);
        }

        public void Deserialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            Name = SerializationUtils.DeserializeString(data, ref offset);
            offset += ByteConverter.ReturnFromStream(data, offset, out X);
            offset += ByteConverter.ReturnFromStream(data, offset, out Y);

            var size = X * Y;
            offset += ByteConverter.ReturnFromStream(data, offset, size, out byte[] content);
            Content = new GameTileContentType[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                Content[i] = (GameTileContentType)content[i];
            }
            offset += ByteConverter.ReturnFromStream(data, offset, size, out Levels);
            
            offset += ByteConverter.ReturnFromStream(data, offset, out Selected);
        }

        public static UserBoardState GetInitial(Vector2Int boardSize, string name)
        {
            var size = boardSize.x * boardSize.y;
            var result = new UserBoardState
            {
                Version = 1,
                Name = name,
                X = (byte)boardSize.x,
                Y = (byte)boardSize.y,
                Content = new GameTileContentType[size],
                Levels = new byte[size],
            };
            result.Content[0] = GameTileContentType.SpawnPoint;
            result.Content[^1] = GameTileContentType.Destination;
            return result;
        }
    }
}