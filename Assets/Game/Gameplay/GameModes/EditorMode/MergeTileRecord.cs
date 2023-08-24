using Game.Defend.Tiles;
using System.Collections.Generic;

namespace GamePlay.Modes
{
    public sealed class MergeTileRecord : BaseBoardActionRecord
    {
        private readonly ModifyTileViewController _viewController;
        private readonly IEnumerable<GameTile.ContentInfo> _tilesAround;
        private readonly int _initialLevel;

        public MergeTileRecord(ModifyTileViewController viewController, GameTile selectedTile,
            IEnumerable<GameTile.ContentInfo> tilesAround)
        {
            _viewController = viewController;
            _tilesAround = tilesAround;
            _initialLevel = selectedTile.Content.Level;
        }

        public override void Undo()
        {
            foreach (var tileInfo in _tilesAround)
            {
                tileInfo.Tile.Content.ChangeType(tileInfo.Type);
                _viewController.ReplaceTile(tileInfo.Tile, tileInfo.Level);
            }
            _viewController.ReplaceTile(_initialLevel);
        }
    }
}
