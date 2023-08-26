

using Game.Defend.Tiles;

namespace GamePlay.Modes
{
    public sealed class DestroyTileRecord : BaseBoardActionRecord
    {
        private readonly ModifyTileViewController _viewController;
        private readonly GameTile _selectedTile;
        private readonly GameTileContentType _tileType;
        private readonly int _tileLevel;

        public DestroyTileRecord(ModifyTileViewController viewController, GameTile selectedTile)
        {
            _viewController = viewController;
            _selectedTile = selectedTile;
            _tileType = selectedTile.Content.Type;
            _tileLevel = selectedTile.Content.Level;
        }

        public override void Undo()
        {
            _selectedTile.Content.ChangeType(_tileType);
            ViewControllerRouter.OnTileClicked(_selectedTile);
            _viewController.ReplaceTile(_tileLevel);
        }
    }
}
