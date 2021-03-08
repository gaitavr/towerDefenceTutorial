using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardSerializer
{
    public IEnumerable<FileContext> GetAllSaves()
    {
        var path = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}";
        if(Directory.Exists(path) == false)
            yield break;
        foreach (var file in Directory.GetFiles(path))
        {
            if(Path.HasExtension(file) == false || Path.GetExtension(file) != Serialization.DEFEND_EXTENSION)
                continue;
            yield return new FileContext(new FileInfo(file));
        }
    }
    
    public void Save(BoardData data, string fileName)
    {
        var savePath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
        File.WriteAllBytes(savePath, data.Serialize());
        UnityEditor.EditorUtility.RevealInFinder(savePath);
    }

    public BoardData Load(string fileName)
    {
        var savePath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
        if (File.Exists(savePath) == false)
            return null;
        return BoardData.Deserialize(File.ReadAllBytes(savePath));
    }
}