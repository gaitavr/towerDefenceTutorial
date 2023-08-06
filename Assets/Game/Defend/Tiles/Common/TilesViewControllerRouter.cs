using System.Collections.Generic;
using System.Linq;

namespace Game.Defend.Tiles
{
    public class TilesViewControllerRouter
    {
        private readonly GameTileRaycaster _raycaster;
        private readonly List<IGameTileViewController> _viewControllers;

        private IGameTileViewController _controllerInProgress;

        public TilesViewControllerRouter(GameTileRaycaster raycaster)
        {
            _raycaster = raycaster;
            _viewControllers = new List<IGameTileViewController>(6);
            _raycaster.TileClicked += OnTileClicked;
        }

        private void OnTileClicked(GameTileContent tile)
        {
            var viewController = _viewControllers.FirstOrDefault(v => v.HandlingType == tile.Type);
            if (viewController != null)
            {
                if (_controllerInProgress == null)
                {
                    _controllerInProgress = viewController;
                    _controllerInProgress.Show(tile);
                    return;
                }

                if (_controllerInProgress == viewController)
                {
                    if(_controllerInProgress.CurrentContent == tile)
                        return;
                    else
                    {
                        _controllerInProgress.ChangeTarget(tile);
                    }
                }
                
                if(_controllerInProgress != null && _controllerInProgress.IsBusy)
                    return;
                
                _controllerInProgress?.Hide();
                _controllerInProgress = viewController;
                _controllerInProgress.Show(tile);
            }
        }

        public void Register(IGameTileViewController viewController)
        {
            _viewControllers.Add(viewController);
            viewController.Finished += OnControllerFinished;
        }

        private void OnControllerFinished(IGameTileViewController _)
        {
            _controllerInProgress?.Hide();
            _controllerInProgress = null;
        }

        public void Unregister(IGameTileViewController viewController)
        {
            _viewControllers.Remove(viewController);
            viewController.Finished -= OnControllerFinished;
        }
    }
}