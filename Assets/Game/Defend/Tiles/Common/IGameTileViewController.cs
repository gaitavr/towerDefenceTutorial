using Cysharp.Threading.Tasks;

namespace Game.Defend.Tiles
{
    public interface IGameTileViewController
    {
        GameTileContentType HandlingType { get; }

        UniTask Show(GameTileContent tile);
        void Hide();
    }
}