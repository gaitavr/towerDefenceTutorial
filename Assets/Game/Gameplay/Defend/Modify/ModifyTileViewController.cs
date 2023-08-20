using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Core.GamePlay;
using Utils.Assets;

namespace Game.Defend.Tiles
{
    public class ModifyTileViewController : GameTileViewController, ITilesModifier
    {
        public ModifyTileViewController(GameTileContentType handlingType, GameTileContentFactory contentFactory, 
            GameBoard gameBoard, GamePlayUI gamePlayUI, TilesViewControllerRouter router) : base(contentFactory, gameBoard, gamePlayUI)
        {
            HandlingType = handlingType;
            router.Register(this);
        }

        public override async UniTask Show(GameTile gameTile)
        {
            if (_selectedTile == gameTile)
                return;

            _selectedTile = gameTile;

            if (_disposableUI == null)
            {
                var subView = await LoadSubView<TilesModifyUI>(AssetsConstants.TilesModifier);
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
        
        void ITilesModifier.DoWithTile(TileModifyActions actionType)
        {
            switch (actionType)
            {
                case TileModifyActions.Upgrade:
                    UpgradeTile();
                    break;
                case TileModifyActions.Merge:
                    OnMergeClicked();
                    break;
                case TileModifyActions.Destroy:
                    DestroyTile();
                    break;
                default: 
                    throw new ArgumentOutOfRangeException($"No handle for action type: {actionType}");
            }
        }

        private void UpgradeTile()
        {
            if (_contentFactory.IsNextUpgradeAllowed(_selectedTile.Content))//TODO check money
            {
                ReplaceTile(_selectedTile.Content.Level + 1);
            }
        }

        private void OnMergeClicked()
        {
            var tilesAround = _gameBoard.GetTilesAround(_selectedTile)
                .Where(t => t.Content.Type == _selectedTile.Content.Type);
            
            var currentLevel = _selectedTile.Content.Level;
            foreach (var t in tilesAround)
            {
                currentLevel += t.Content.Level + 1;//levels start from 0
                _gameBoard.DestroyTile(t);
            }
            
            if(_selectedTile.Content.Level == currentLevel)
                return;

            ReplaceTile(currentLevel);
        }

        private void ReplaceTile(int level)
        {
            var newTile = _contentFactory.Get(_selectedTile.Content.Type, level);
            _gameBoard.DestroyTile(_selectedTile);
            _gameBoard.TryBuild(_selectedTile, newTile);
        }
        
        private void DestroyTile()
        {
            _gameBoard.DestroyTile(_selectedTile);
            RaiseFinished();
        }
    }
}