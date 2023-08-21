using System;
using System.Drawing;
using UnityEngine;

namespace Utils.Serialization
{
    [Serializable]
    public class BoardData
    {
        public int Version;
        public int AccountId;
        public byte X;
        public byte Y;
        public GameTileContentType[] Content;
        public byte[] Levels;

        public byte[] Serialize()
        {
            var result = new byte[sizeof(int) + sizeof(int) + sizeof(byte) * 2 +
                                  sizeof(byte) * Content.Length + sizeof(byte) * Levels.Length];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream(AccountId, result, offset);
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

        public static BoardData Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out int version);
            if (version != Constants.VERSION)
                return null;

            var result = new BoardData();

            offset += ByteConverter.ReturnFromStream(data, offset, out result.AccountId);
            offset += ByteConverter.ReturnFromStream(data, offset, out result.X);
            offset += ByteConverter.ReturnFromStream(data, offset, out result.Y);

            int size = result.X * result.Y;
            offset += ByteConverter.ReturnFromStream(data, offset, size, out byte[] content);
            offset += ByteConverter.ReturnFromStream(data, offset, size, out result.Levels);

            result.Content = new GameTileContentType[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                result.Content[i] = (GameTileContentType)content[i];
            }

            return result;
        }

        public static BoardData GetEmpty(Vector2Int boardSize)
        {
            var size = boardSize.x * boardSize.y;
            var result = new BoardData
            {
                X = (byte)boardSize.x,
                Y = (byte)boardSize.y,
                Content = new GameTileContentType[size],
                Levels = new byte[size]
            };
            result.Content[0] = GameTileContentType.SpawnPoint;
            result.Content[^1] = GameTileContentType.Destination;
            return result;
        }
    }
}