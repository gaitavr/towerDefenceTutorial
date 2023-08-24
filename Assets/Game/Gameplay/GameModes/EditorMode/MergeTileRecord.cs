using Game.Defend.Tiles;
using System.Collections.Generic;
using System.Linq;

namespace GamePlay.Modes
{
    public sealed class MergeTileRecord : BaseBoardActionRecord
    {
        private readonly ModifyTileViewController _viewController;
        private readonly List<ContentInfo> _tilesAround;
        private readonly int _levelBeforeMerge;

        public MergeTileRecord(ModifyTileViewController viewController, int levelBeforeMerge,
            IEnumerable<GameTile> tilesAround)
        {
            _viewController = viewController;
            _tilesAround = tilesAround.Select(t => GetInfo(t)).ToList();
            _levelBeforeMerge = levelBeforeMerge;
        }

        public override void Undo()
        {
            foreach (var tileInfo in _tilesAround)
            {
                tileInfo.Tile.Content.ChangeType(tileInfo.Type);
                _viewController.ReplaceTile(tileInfo.Tile, tileInfo.Level);
            }
            _viewController.ReplaceTile(_levelBeforeMerge);
        }

        private ContentInfo GetInfo(GameTile tile)
        {
            return new ContentInfo()
            {
                Type = tile.Content.Type,
                Level = tile.Content.Level,
                Tile = tile
            };
        }

        private struct ContentInfo
        {
            public GameTileContentType Type;
            public int Level;
            public GameTile Tile;
        }
    }
}
