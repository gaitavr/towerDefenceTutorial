using System;
using Cysharp.Threading.Tasks;

namespace Game.Defend.Tiles
{
    public interface IGameTileViewController
    {
        event Action<IGameTileViewController> Finished;

        GameTileContentType HandlingType { get; }
        GameTile CurrentTile { get; }
        bool IsBusy { get; }

        UniTask Show(GameTile tile);
        void Hide();
    }
}