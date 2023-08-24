
using Core;

namespace GamePlay.Modes
{
    public sealed class BuildTileCommand : BaseTileCommand
    {
        private readonly GameTile _tile;

        public BuildTileCommand(GameTile tile)
        {
            _tile = tile;
        }

        public override void Undo()
        {
            UserContainer.RefundAfterBuild(_tile.Content.Type);
            GameBoard.DestroyTile(_tile);
        }
    }
}
