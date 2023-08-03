using System;
using UnityEngine;

namespace Game.Defend.Tiles
{
    public class GameTileRaycaster
    {
        private readonly Camera _camera;
        private readonly GameBoard _gameBoard;
        
        public Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

        public event Action<GameTileContent> TileClicked;

        public GameTileRaycaster(Camera camera, GameBoard gameBoard)
        {
            _camera = camera;
            _gameBoard = gameBoard;
        }
        
        /// <summary>
        /// Raycaster should go before builder to prevent immediate processing of tile
        /// </summary>
        public void GameUpdate()
        {
            if (IsPointerDown() == false) 
                return;
            
            var hit = Raycast();
            if(hit == null)
                return;
                
            var boardTile = _gameBoard.GetTile(hit.Value.point);
            if (boardTile != null)
            {
                TileClicked?.Invoke(boardTile.Content);
            }
            else
            {
                if(hit.Value.transform.TryGetComponent<GameTileContent>(out var tileContent))
                    TileClicked?.Invoke(tileContent);
            }
        }
        
        public GameTile GetTile()
        {
            var hit = Raycast();
            return hit != null ? _gameBoard.GetTile(hit.Value.point) : null;
        }

        private RaycastHit? Raycast()
        {
            if (Physics.Raycast(TouchRay, out var hit, float.MaxValue, 1))
                return hit;
            return null;
        }
        
        public bool IsPointerDown()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            return Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began;
#endif
        }
    }
}