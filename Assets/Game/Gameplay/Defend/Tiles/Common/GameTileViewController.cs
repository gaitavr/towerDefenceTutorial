using System;
using Cysharp.Threading.Tasks;
using GamePlay;
using Utils.Assets;

namespace Game.Defend.Tiles
{
    public abstract class GameTileViewController
    {
        protected readonly GameTileContentFactory _contentFactory;
        protected readonly GameBoard _gameBoard;
        protected readonly GamePlayUI _gamePlayUI;

        protected IDisposable _disposableUI;
        protected GameTile _selectedTile;

        protected GameTileViewController(GameTileContentFactory contentFactory,
            GameBoard gameBoard, GamePlayUI gamePlayUI)
        {
            _contentFactory = contentFactory;
            _gameBoard = gameBoard;
            _gamePlayUI = gamePlayUI;
        }

        public event Action<GameTileViewController> Finished;

        public GameTileContentType HandlingType { get; protected set; }

        public abstract UniTask Show(GameTile tile);
        public abstract void Hide();

        protected async UniTask<T> LoadSubView<T>(string assetKey)
        {
            var assetsLoader = new LocalAssetLoader();
            var subView = await assetsLoader.LoadDisposable<T>(assetKey,
                _gamePlayUI.ActionsSocket);
            _disposableUI = subView;
            return subView.Value;
        }

        protected void RaiseFinished()
        {
            Finished?.Invoke(this);
        }
    }
}