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
                
            var boardTile = GetTile(hit.Value);
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
            return hit != null ? GetTile(hit.Value) : null;
        }
        
        private GameTile GetTile(RaycastHit hit)
        {
            var x = (int) (hit.point.x + _gameBoard.X * 0.5f);
            var y = (int) (hit.point.z + _gameBoard.Y * 0.5f);
            if (x >= 0 && x < _gameBoard.X && y >= 0 && y < _gameBoard.Y)
                return _gameBoard.GetTile(x, y);
            return null;
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