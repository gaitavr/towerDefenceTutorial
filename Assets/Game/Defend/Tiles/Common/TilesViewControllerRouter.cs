using System.Collections.Generic;
using System.Linq;

namespace Game.Defend.Tiles
{
    public class TilesViewControllerRouter
    {
        private readonly GameTileRaycaster _raycaster;
        private readonly List<IGameTileViewController> _viewControllers;

        private IGameTileViewController _current;

        public TilesViewControllerRouter(GameTileRaycaster raycaster)
        {
            _raycaster = raycaster;
            _viewControllers = new List<IGameTileViewController>(6);
            _raycaster.TileClicked += OnTileClicked;
        }

        private void OnTileClicked(GameTileContentType tileType)
        {
            var viewController = _viewControllers.FirstOrDefault(v => v.HandlingType == tileType);
            if (viewController != null)
            {
                _current?.Hide();
                _current = viewController;
                _current.Show();
            }
        }

        public void Register(IGameTileViewController viewController)
        {
            _viewControllers.Add(viewController);
        }

        public void Unregister(IGameTileViewController viewController)
        {
            _viewControllers.Remove(viewController);
        }
    }
}