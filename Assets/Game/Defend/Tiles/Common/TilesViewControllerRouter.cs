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

        private void OnTileClicked(GameTileContent tile)
        {
            var viewController = _viewControllers.FirstOrDefault(v => v.HandlingType == tile.Type);
            if (viewController != null)
            {
                if(viewController == _current)
                    return;
                _current?.Hide();
                _current = viewController;
                _current.Show(tile);
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