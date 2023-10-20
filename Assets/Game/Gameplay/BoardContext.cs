using Core;
using UnityEngine;

namespace Gameplay
{
    public sealed class BoardContext
    {
        public string Name { get; private set; }
        public Vector2Int Size { get; private set; }
        public bool IsSelected { get; private set; }

        public BoardContext(string name, Vector2Int size, bool isSelected)
        {
            Name = name;
            Size = size;
            IsSelected = isSelected;
        }

        public BoardContext(UserBoardState boardData)
        {
            Name = boardData.Name;
            Size = new Vector2Int(boardData.X, boardData.Y);
            IsSelected = boardData.Selected;
        }
    }
}
