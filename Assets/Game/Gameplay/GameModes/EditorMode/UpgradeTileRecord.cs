using Core;
using Game.Defend.Tiles;

namespace GamePlay.Modes
{
    public sealed class UpgradeTileRecord : BaseBoardActionRecord
    {
        private readonly ModifyTileViewController _viewController;
        private readonly GameTile _selectedTile;
        private readonly int _level;

        public UpgradeTileRecord(ModifyTileViewController viewController, GameTile selectedTile)
        {
            _viewController = viewController;
            _selectedTile = selectedTile;
            _level = selectedTile.Content.Level;
        }

        public override void Undo()
        {
            ViewControllerRouter.OnTileClicked(_selectedTile);
            UserContainer.RefundAfterUpgrade(_viewController.HandlingType, _level);
            _viewController.ReplaceTile(_level - 1);
        }
    }
}
