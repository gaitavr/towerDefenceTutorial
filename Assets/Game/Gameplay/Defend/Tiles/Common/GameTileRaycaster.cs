using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Defend
{
    public class GameTileRaycaster
    {
        private readonly Camera _camera;
        private readonly GameBoard _gameBoard;

        private bool _isMuted;
        
        public Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

        public event Action<GameTile> TileClicked;

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
            if (boardTile != null && _isMuted == false)
                TileClicked?.Invoke(boardTile);
        }
        
        public GameTile GetTile()
        {
            var hit = Raycast();
            return hit != null ? _gameBoard.GetTile(hit.Value.point) : null;
        }

        private RaycastHit? Raycast()
        {
            var isOverUI = EventSystem.current.IsPointerOverGameObject();
            if (Physics.Raycast(TouchRay, out var hit, float.MaxValue) && isOverUI == false)
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

        public void Mute()
        {
            _isMuted = true;
        }

        public void UnMute()
        {
            _isMuted = false;
        }
    }
}