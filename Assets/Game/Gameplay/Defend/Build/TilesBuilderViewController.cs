using System;
using Core.Pause;
using Cysharp.Threading.Tasks;
using GamePlay;
using UnityEngine;
using Utils.Assets;
using Object = UnityEngine.Object;

namespace Game.Defend.Tiles
{
    public class TilesBuilderViewController : GameTileViewController, ITilesBuilder
    {
        private readonly GameTileRaycaster _raycaster;

        private GameTileContent _tempTile;
        private bool _isActive;

        private PauseManager PauseManager => ProjectContext.I.PauseManager;
        private bool IsPaused => PauseManager.IsPaused;

        public TilesBuilderViewController(GameTileContentFactory contentFactory, GameTileRaycaster raycaster,
            GameBoard gameBoard, GamePlayUI gamePlayUI, TilesViewControllerRouter router) : base(contentFactory, gameBoard, gamePlayUI)
        {
            _raycaster = raycaster;
            HandlingType = GameTileContentType.Empty;
            router.Register(this);
        }

        public override async UniTask Show(GameTile gameTile)
        {
            if (_selectedTile == gameTile)
                return;

            _selectedTile = gameTile;

            if (_disposableUI == null)
            {
                var subView = await LoadSubView<TilesBuilderUI>(AssetsConstants.TilesBuilder);
                foreach (var button in subView.Buttons)
                {
                    button.Initialize(this);
                }
            }
        }

        public override void Hide()
        {
            _selectedTile = null;
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
                _raycaster.UnMute();
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
            _raycaster.Mute();
        }
    }
}