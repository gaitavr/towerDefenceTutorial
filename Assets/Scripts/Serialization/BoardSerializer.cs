using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    public void Delete(string fileName)
    {
        var savePath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
        if(File.Exists(savePath))
            File.Delete(savePath);
    }
    
    public void Save(BoardData data, string fileName)
    {
        var formatter = new BinaryFormatter();
        var savePath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
        var savePath2 = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}.ttt";
        File.WriteAllBytes(savePath, data.Serialize());
        using (FileStream stream = File.Create(savePath2))
        {
            formatter.Serialize(stream, data);
        }
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