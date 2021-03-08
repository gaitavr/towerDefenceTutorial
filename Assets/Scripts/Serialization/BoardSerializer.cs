using System.IO;
using UnityEngine;

public class BoardSerializer
{
    public void Save(BoardData data, string fileName)
    {
        var savePath = $"{Application.persistentDataPath}/{fileName}.def";
        File.WriteAllBytes(savePath, data.Serialize());
        UnityEditor.EditorUtility.RevealInFinder(savePath);
    }

    public BoardData Load(string fileName)
    {
        var savePath = $"{Application.persistentDataPath}/{fileName}.def";
        if (File.Exists(savePath) == false)
            return null;
        return BoardData.Deserialize(File.ReadAllBytes(savePath));
    }
}