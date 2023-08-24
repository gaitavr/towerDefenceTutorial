using Core;
using Game.Defend.Tiles;

namespace GamePlay.Modes
{
    public sealed class UpgradeTileCommand : BaseTileCommand
    {
        private readonly ModifyTileViewController _viewController;
        private readonly int _level;

        public UpgradeTileCommand(ModifyTileViewController viewController, int level)
        {
            _viewController = viewController;
            _level = level;
        }

        public override void Undo()
        {
            UserContainer.RefundAfterUpgrade(_viewController.HandlingType, _level);
            _viewController.ReplaceTile(_level - 1);
        }
    }
}
