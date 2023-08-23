using System.Text;
using UnityEngine;

namespace Utils.Serialization
{
    public sealed class BoardData : ISerializable
    {
        public int Version;
        public string Name;
        public byte X;
        public byte Y;
        public GameTileContentType[] Content;
        public byte[] Levels;

        public byte[] Serialize()
        {
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            var result = new byte[sizeof(int) + sizeof(byte) + nameBytes.Length + sizeof(byte) * 2 +
                                  sizeof(byte) * Content.Length + sizeof(byte) * Levels.Length];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream((byte)nameBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(nameBytes, result, offset);
            offset += ByteConverter.AddToStream(X, result, offset);
            offset += ByteConverter.AddToStream(Y, result, offset);

            foreach (var c in Content)
            {
                offset += ByteConverter.AddToStream((byte)c, result, offset);
            }

            foreach (var l in Levels)
            {
                offset += ByteConverter.AddToStream(l, result, offset);
            }

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);

            offset += ByteConverter.ReturnFromStream(data, offset, out byte nameSize);
            offset += ByteConverter.ReturnFromStream(data, offset, nameSize, out var nameBytes);
            Name = Encoding.UTF8.GetString(nameBytes);
            offset += ByteConverter.ReturnFromStream(data, offset, out X);
            offset += ByteConverter.ReturnFromStream(data, offset, out Y);

            int size = X * Y;
            offset += ByteConverter.ReturnFromStream(data, offset, size, out byte[] content);
            offset += ByteConverter.ReturnFromStream(data, offset, size, out Levels);

            Content = new GameTileContentType[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                Content[i] = (GameTileContentType)content[i];
            }
        }

        public static BoardData GetInitial(Vector2Int boardSize)
        {
            var size = boardSize.x * boardSize.y;
            var result = new BoardData
            {
                Version = 1,
                Name = $"test{size}",
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