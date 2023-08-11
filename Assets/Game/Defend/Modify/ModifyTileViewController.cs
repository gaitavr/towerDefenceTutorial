using System;
using System.Linq;
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
        private GameTile _selectedTile;

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
        public GameTile CurrentTile => _selectedTile;

        public async UniTask Show(GameTile gameTile)
        {
            if (_selectedTile == gameTile)
                return;

            _selectedTile = gameTile;

            if (_disposableUI == null)
            {
                var assetsLoader = new LocalAssetLoader();
                var tilesModifierUI = await assetsLoader.LoadDisposable<TilesModifyUI>(AssetsConstants.TilesModifier,
                    _gamePlayUI.ActionsSocket);
                _disposableUI = tilesModifierUI;
                foreach (var button in tilesModifierUI.Value.Buttons)
                {
                    button.Initialize(this);
                }
            }
        }

        public void Hide()
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
            var tilesAround = _gameBoard.GetTilesAround(_selectedTile.Content)
                .Where(t => t.Content.Type == _selectedTile.Content.Type);
            
            var currentLevel = _selectedTile.Content.Level;
            foreach (var t in tilesAround)
            {
                currentLevel += t.Content.Level;
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
            Finished?.Invoke(this);
        }
    }
}