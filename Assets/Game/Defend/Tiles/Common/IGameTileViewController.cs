using System;
using Cysharp.Threading.Tasks;

namespace Game.Defend.Tiles
{
    public interface IGameTileViewController
    {
        event Action<IGameTileViewController> Finished;

        GameTileContentType HandlingType { get; }
        GameTileContent CurrentContent { get; }
        bool IsBusy { get; }

        UniTask Show(GameTileContent tile);
        void ChangeTarget(GameTileContent gameTile);
        void Hide();
    }
}