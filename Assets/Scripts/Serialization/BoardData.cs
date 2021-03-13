using System;

[Serializable]
public class BoardData
{
    public int Version;
    public int AccountId;
    public byte X;
    public byte Y;
    public GameTileContentType[] Content;

    public byte[] Serialize()
    {
        var result = new byte[sizeof(int) + sizeof(int) + sizeof(byte) * 2 +
                              sizeof(byte) * Content.Length];

        var offset = 0;
        offset += ByteConverter.AddToStream(Version, result, offset);
        offset += ByteConverter.AddToStream(AccountId, result, offset);
        offset += ByteConverter.AddToStream(X, result, offset);
        offset += ByteConverter.AddToStream(Y, result, offset);
        foreach (var c in Content)
        {
            offset += ByteConverter.AddToStream((byte) c, result, offset);
        }

        return result;
    }

    public static BoardData Deserialize(byte[] data)
    {
        var offset = 0;

        offset += ByteConverter.ReturnFromStream(data, offset, out int version);
        if (version != Serialization.VERSION)
        {
            return null;
        }

        var result = new BoardData();

        offset += ByteConverter.ReturnFromStream(data, offset, out result.AccountId);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.X);
        offset += ByteConverter.ReturnFromStream(data, offset, out result.Y);
        offset += ByteConverter.ReturnFromStream(data, offset, data.Length - offset, out byte[] content);

        result.Content = new GameTileContentType[content.Length];
        for (var i = 0; i < content.Length; i++)
        {
            result.Content[i] = (GameTileContentType) content[i];
        }

        return result;
    }
}
