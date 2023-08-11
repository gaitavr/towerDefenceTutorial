using System;
using Core.Pause;
using Cysharp.Threading.Tasks;
using Game.Core.GamePlay;
using UnityEngine;
using Utils.Assets;
using Object = UnityEngine.Object;

namespace Game.Defend.Tiles
{
    public class TilesBuilderViewController : ITilesBuilder, IGameTileViewController
    {
        private readonly GameTileContentFactory _contentFactory;
        private readonly GameTileRaycaster _raycaster;
        private readonly GameBoard _gameBoard;
        private readonly GamePlayUI _gamePlayUI;

        private GameTileContent _tempTile;
        private bool _isActive;
        private IDisposable _disposableUI;

        private PauseManager PauseManager => ProjectContext.I.PauseManager;
        private bool IsPaused => PauseManager.IsPaused;

        public TilesBuilderViewController(GameTileContentFactory contentFactory, GameTileRaycaster raycaster,
            GameBoard gameBoard, GamePlayUI gamePlayUI, TilesViewControllerRouter router)
        {
            _contentFactory = contentFactory;
            _raycaster = raycaster;
            _gameBoard = gameBoard;
            _gamePlayUI = gamePlayUI;
            router.Register(this);
        }

        public event Action<IGameTileViewController> Finished;
        GameTileContentType IGameTileViewController.HandlingType => GameTileContentType.Empty;
        public GameTile CurrentTile { get; private set; }

        async UniTask IGameTileViewController.Show(GameTile gameTile)
        {
            if (CurrentTile == gameTile)
                return;

            CurrentTile = gameTile;

            if (_disposableUI == null)
            {
                var assetsLoader = new LocalAssetLoader();
                var tilesBuilderUI = await assetsLoader.LoadDisposable<TilesBuilderUI>(AssetsConstants.TilesBuilder,
                    _gamePlayUI.ActionsSocket);
                _disposableUI = tilesBuilderUI;
                foreach (var button in tilesBuilderUI.Value.Buttons)
                {
                    button.Initialize(this);
                }
            }
        }

        void IGameTileViewController.Hide()
        {
            CurrentTile = null;
            _disposableUI.Dispose();
            _disposableUI = null;
        }

        public void GameUpdate()
        {
            if (_isActive == false || IsPaused)
                return;

            if (_tempTile != null)
                ProcessBuilding();
        }

        private void ProcessBuilding()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(_raycaster.TouchRay, out var position))
                _tempTile.transform.position = _raycaster.TouchRay.GetPoint(position);

            if (_raycaster.IsPointerDown())
            {
                var tile = _raycaster.GetTile();
                if (tile == null || _gameBoard.TryBuild(tile, _tempTile) == false)
                    Object.Destroy(_tempTile.gameObject);

                _tempTile = null;
            }
        }

        public void SetActive(bool isActive) => _isActive = isActive;

        void ITilesBuilder.SelectBuilding(GameTileContentType type)
        {
            if (IsPaused)
            {
                PauseManager.ShowHint();
                return;
            }

            if(_isActive == false)
                return;
            
            //TODO check money
            _tempTile = _contentFactory.Get(type);
        }
    }
}