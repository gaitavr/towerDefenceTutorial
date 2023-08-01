using System;
using Core.Pause;
using Cysharp.Threading.Tasks;
using Game.Core.GamePlay;
using UnityEngine;
using Utils.Assets;
using Object = UnityEngine.Object;

namespace Game.Defend.TilesBuilder
{
    public class TilesBuilderViewController : ITilesBuilder
    {
        private readonly GameTileContentFactory _contentFactory;
        private readonly Camera _camera;
        private readonly GameBoard _gameBoard;
        private readonly GamePlayUI _gamePlayUI;

        private GameTileContent _tempTile;
        private bool _isActive;
        private bool _isShown;
        private IDisposable _disposableUI;//TODO use it

        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);
        private PauseManager PauseManager => ProjectContext.I.PauseManager;
        private bool IsPaused => PauseManager.IsPaused;
        private bool CanBuild => _isActive && _isShown;

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
            if (_isShown)
                return;
            _isShown = true;
            var assetsLoader = new LocalAssetLoader();
            var tilesBuilderUI = await assetsLoader.LoadDisposable<TilesBuilderUI>(AssetsConstants.TilesBuilder, 
                _gamePlayUI.ActionsSocket);
            _disposableUI = tilesBuilderUI;
            foreach (var button in tilesBuilderUI.Value.Buttons)
            {
                button.Initialize(this);
            }
        }

        public void GameUpdate()
        {
            if (CanBuild == false || IsPaused)
                return;

            if (_tempTile != null)
                ProcessBuilding();
            
            //TODO process upgrade and destroy
            // if (_pendingTile == null)
            // {
            //     ProcessDestroying();
            // }
        }

        private void ProcessBuilding()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(TouchRay, out var position))
                _tempTile.transform.position = TouchRay.GetPoint(position);

            if (IsPointerDown())
            {
                var tile = _gameBoard.GetTile(TouchRay);
                if (tile == null || _gameBoard.TryBuild(tile, _tempTile) == false)
                    Object.Destroy(_tempTile.gameObject);

                _tempTile = null;
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
            if (IsPointerDown())
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
            if (IsPointerDown())
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

        private bool IsPointerDown()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            return Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began;
#endif
        }

        public void SetActive(bool isActive) => _isActive = isActive;

        void ITilesBuilder.SelectBuilding(GameTileContentType type)
        {
            if (IsPaused)
            {
                PauseManager.ShowHint();
                return;
            }

            if(CanBuild == false)
                return;
            
            //TODO check money
            _tempTile = _contentFactory.Get(type);
        }
    }
}