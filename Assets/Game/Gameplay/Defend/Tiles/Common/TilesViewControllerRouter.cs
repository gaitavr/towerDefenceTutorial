using System.Collections.Generic;
using System.Linq;

namespace Game.Defend.Tiles
{
    public sealed class TilesViewControllerRouter
    {
        private readonly GameTileRaycaster _raycaster;
        private readonly List<GameTileViewController> _viewControllers;

        private GameTileViewController _controllerInProgress;

        public TilesViewControllerRouter(GameTileRaycaster raycaster)
        {
            _raycaster = raycaster;
            _viewControllers = new List<GameTileViewController>(6);
            _raycaster.TileClicked += OnTileClicked;
        }

        public void OnTileClicked(GameTile tile)
        {
            var viewController = _viewControllers.FirstOrDefault(v => v.HandlingType == tile.Content.Type);
            if (viewController != null)
            {
                if (_controllerInProgress == null)
                {
                    _controllerInProgress = viewController;
                    _controllerInProgress.Show(tile);
                    return;
                }

                if (_controllerInProgress != viewController)
                    _controllerInProgress.Hide();

                _controllerInProgress = viewController;
                _controllerInProgress.Show(tile);
            }
        }

        public void Register(GameTileViewController viewController)
        {
            _viewControllers.Add(viewController);
            viewController.Finished += OnControllerFinished;
        }

        private void OnControllerFinished(GameTileViewController _)
        {
            _controllerInProgress?.Hide();
            _controllerInProgress = null;
        }

        public void Unregister(GameTileViewController viewController)
        {
            _viewControllers.Remove(viewController);
            viewController.Finished -= OnControllerFinished;
        }
    }
}