
using Core;
using GamePlay.Defend;

namespace GamePlay.Modes
{
    public sealed class BuildTileRecord : BaseBoardActionRecord
    {
        private readonly GameTile _tile;

        public BuildTileRecord(GameTile tile)
        {
            _tile = tile;
        }

        public override void Undo()
        {
            UserContainer.RefundAfterBuild(_tile.Content.Type);
            GameBoard.DestroyTile(_tile);
            ViewControllerRouter.OnTileClicked(_tile);
        }
    }
}
