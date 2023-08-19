using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utils.Serialization
{
    public class BoardSerializer
    {
        public IEnumerable<FileContext> GetAllSaves()
        {
            var path = $"{Application.persistentDataPath}/{Constants.DEFEND_PATH}";
            if (Directory.Exists(path) == false)
                yield break;
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.HasExtension(file) == false || Path.GetExtension(file) != Constants.DEFEND_EXTENSION)
                    continue;
                yield return new FileContext(new FileInfo(file));
            }
        }

        public void Delete(string fileName)
        {
            var savePath = $"{Application.persistentDataPath}/{Constants.DEFEND_PATH}/{fileName}{Constants.DEFEND_EXTENSION}";
            if (File.Exists(savePath))
                File.Delete(savePath);
        }

        public void Save(BoardData data, string fileName)
        {
            var formatter = new BinaryFormatter();
            var directoryPath = $"{Application.persistentDataPath}/{Constants.DEFEND_PATH}";
            if (Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);
            var savePath = $"{directoryPath}/{fileName}{Constants.DEFEND_EXTENSION}";
            File.WriteAllBytes(savePath, data.Serialize());
#if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(savePath);
#endif
        }

        public BoardData Load(string fileName)
        {
            var savePath = $"{Application.persistentDataPath}/{Constants.DEFEND_PATH}/{fileName}{Constants.DEFEND_EXTENSION}";
            if (File.Exists(savePath) == false)
                return null;
            return BoardData.Deserialize(File.ReadAllBytes(savePath));
        }
    }
}