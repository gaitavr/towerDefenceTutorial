using Core.Pause;
using Cysharp.Threading.Tasks;
using Game.Core.GamePlay;
using UnityEngine;

namespace Game.Defend.TilesBuilder
{
    public class TilesBuilderViewController
    {
        private readonly GameTileContentFactory _contentFactory;
        private readonly Camera _camera;
        private readonly GameBoard _gameBoard;
        private readonly GamePlayUI _gamePlayUI;

        private GameTileContent _pendingTile;
        private bool _isEnabled;
        private bool _isActive;

        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);
        private PauseManager PauseManager => ProjectContext.I.PauseManager;
        private bool IsPaused => PauseManager.IsPaused;

        public TilesBuilderViewController(GameTileContentFactory contentFactory, Camera camera, GameBoard gameBoard,
            GamePlayUI gamePlayUI)
        {
            _contentFactory = contentFactory;
            _camera = camera;
            _gameBoard = gameBoard;
            _gamePlayUI = gamePlayUI;
        }

        public async UniTask Show()
        {
            if (_isActive)
                return;
            _isActive = true;
            //TODO create sub views and pass them to game play UI

        }

        public void GameUpdate()
        {
            if (_isEnabled == false || IsPaused || _isActive == false)
                return;

            if (_pendingTile == null)
            {
                ProcessDestroying();
            }
            else
            {
                ProcessBuilding();
            }
        }

        private void ProcessBuilding()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(TouchRay, out var position))
            {
                _pendingTile.transform.position = TouchRay.GetPoint(position);
            }

            if (IsPointerUp())
            {
                var tile = _gameBoard.GetTile(TouchRay);
                if (tile == null || _gameBoard.TryBuild(tile, _pendingTile) == false)
                {
                    Object.Destroy(_pendingTile.gameObject);
                }

                _pendingTile = null;
            }
        }

        private void ProcessDestroying()
        {
            //TODO
            // if (_isDestroyAllowed == false)
            {
                ProcessUpgrade();
                return;
            }
            if (IsPointerUp())
            {
                var tile = _gameBoard.GetTile(TouchRay);
                if (tile != null)
                {
                    _gameBoard.DestroyTile(tile);
                }
            }
        }

        private void ProcessUpgrade()
        {
            if (IsPointerUp())
            {
                var tile = _gameBoard.GetTile(TouchRay);
                if (tile != null && _contentFactory.IsNextUpgradeAllowed(tile.Content))
                {
                    var newTile = _contentFactory.Get(tile.Content.Type, tile.Content.Level + 1);
                    _gameBoard.DestroyTile(tile);
                    _gameBoard.TryBuild(tile, newTile);
                }
            }
        }

        private bool IsPointerUp()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#else
        return Input.touches.Length == 0;
#endif
        }

        public void Enable() => _isEnabled = true;

        public void Disable() => _isEnabled = false;

        private void OnBuildingSelected(GameTileContentType type)
        {
            if (IsPaused)
            {
                PauseManager.ShowHint();
                return;
            }

            //TODO check money
            _pendingTile = _contentFactory.Get(type);
        }
    }
}