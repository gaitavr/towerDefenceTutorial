﻿using System;
using System.Linq;
using Core;
using Cysharp.Threading.Tasks;
using GamePlay.Modes;
using Utils.Assets;

namespace GamePlay.Defend
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
            var level = _selectedTile.Content.Level + 1;
            var isUpgradeAllowed = _contentFactory.IsNextUpgradeAllowed(_selectedTile.Content);
            isUpgradeAllowed &= UserContainer.IsUpgradeAllowed(_selectedTile.Content.Type, level);
            if (isUpgradeAllowed)
            {
                UserContainer.SpendAfterUpgrade(_selectedTile.Content.Type, level);
                ReplaceTile(level);
                BoardActionRecorder?.Record(new UpgradeTileRecord(this, _selectedTile));
            }
        }

        private void OnMergeClicked()
        {
            var tilesAround = _gameBoard.GetTilesAround(_selectedTile)
                .Where(t => t.Content.Type == _selectedTile.Content.Type);

            var currentLevel = _selectedTile.Content.Level;
            var record = new MergeTileRecord(this, currentLevel, tilesAround);

            foreach (var t in tilesAround)
            {
                currentLevel += t.Content.Level + 1;//levels start from 0
                _gameBoard.DestroyTile(t);
            }
            
            if(_selectedTile.Content.Level == currentLevel)
                return;

            BoardActionRecorder?.Record(record);
            ReplaceTile(currentLevel);
        }

        public void ReplaceTile(int level)
        {
            ReplaceTile(_selectedTile, level);
        }

        public void ReplaceTile(GameTile tile, int level)
        {
            var newTile = _contentFactory.Get(tile.Content.Type, level);
            _gameBoard.DestroyTile(tile);
            _gameBoard.TryBuild(tile, newTile);
        }

        private void DestroyTile()
        {
            BoardActionRecorder?.Record(new DestroyTileRecord(this, _selectedTile));
            _gameBoard.DestroyTile(_selectedTile);
            RaiseFinished();
        }
    }
}