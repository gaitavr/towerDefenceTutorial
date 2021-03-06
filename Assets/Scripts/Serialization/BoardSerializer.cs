using System.IO;
using UnityEngine;

namespace Serialization
{
    public class BoardSerializer
    {
        public string Save(BoardData data)
        {
            var savePath = Application.persistentDataPath + "board.def";
            File.WriteAllBytes(savePath, data.Serialize());
            UnityEditor.EditorUtility.RevealInFinder(savePath);
            return savePath;
        }

        public BoardData Load()
        {
            var savePath = Application.persistentDataPath + "board.def";
            if (File.Exists(savePath) == false)
                return null;
            return BoardData.Deserialize(File.ReadAllBytes(savePath));
        }
    }
}