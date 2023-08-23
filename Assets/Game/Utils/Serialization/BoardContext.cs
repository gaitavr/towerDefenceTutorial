using UnityEngine;

namespace Utils.Serialization
{
    public sealed class BoardContext
    {
        public string Name { get; private set; }
        public Vector2Int Size { get; private set; }

        public BoardContext(string name, Vector2Int size)
        {
            Name = name;
            Size = size;
        }

        public BoardContext(BoardData boardData)
        {
            Name = boardData.Name;
            Size = new Vector2Int(boardData.X, boardData.Y);
        }
    }
}
