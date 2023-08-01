using Cysharp.Threading.Tasks;

namespace Game.Defend.Tiles
{
    public interface IGameTileViewController
    {
        GameTileContentType HandlingType { get; }

        UniTask Show();
        void Hide();
    }
}