using System;
using Cysharp.Threading.Tasks;
using Game.Core.GamePlay;
using Utils.Assets;

namespace Game.Defend.Tiles
{
    public class ModifyTileViewController : IGameTileViewController, ITilesModifier
    {
        private readonly GameTileContentFactory _contentFactory;
        private readonly GameBoard _gameBoard;
        private readonly GamePlayUI _gamePlayUI;
        
        private IDisposable _disposableUI;
        private GameTileContent _selectedTile;

        public ModifyTileViewController(GameTileContentType handlingType, GameTileContentFactory contentFactory, 
            GameBoard gameBoard, GamePlayUI gamePlayUI, TilesViewControllerRouter router)
        {
            HandlingType = handlingType;
            _contentFactory = contentFactory;
            _gameBoard = gameBoard;
            _gamePlayUI = gamePlayUI;
            router.Register(this);
        }

        public event Action<IGameTileViewController> Finished;
        public GameTileContentType HandlingType { get; }
        public GameTileContent CurrentContent => _selectedTile;

        public async UniTask Show(GameTileContent gameTile)
        {
            ChangeTarget(gameTile);
            var assetsLoader = new LocalAssetLoader();
            var tilesModifierUI = await assetsLoader.LoadDisposable<TilesModifyUI>(AssetsConstants.TilesModifier, 
                _gamePlayUI.ActionsSocket);
            _disposableUI = tilesModifierUI;
            foreach (var button in tilesModifierUI.Value.Buttons)
            {
                button.Initialize(this);
            }
        }

        public void ChangeTarget(GameTileContent gameTile)
        {
            _selectedTile = gameTile;
        }

        public void Hide()
        {
            _disposableUI.Dispose();
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
            if (_contentFactory.IsNextUpgradeAllowed(_selectedTile))//TODO check money
            {
                var newTile = _contentFactory.Get(_selectedTile.Type, _selectedTile.Level + 1);
                var tile = _gameBoard.DestroyTile(_selectedTile);
                _selectedTile = newTile;
                _gameBoard.TryBuild(tile, newTile);
            }
        }

        private void OnMergeClicked()
        {
            
        }
        
        private void DestroyTile()
        {
            _gameBoard.DestroyTile(_selectedTile);
            Finished?.Invoke(this);
        }
    }
}