using System;
using System.Collections.Generic;
using UnityEngine;

public class TilesBuilder : MonoBehaviour
    {
        [SerializeField]
        private List<BuildButton> _buttons;
        
        private GameTileContentFactory _contentFactory;
        private Camera _camera;
        private GameBoard _gameBoard;
        
        private bool _isEnabled;
        
        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

        private GameTileContent _pendingTile;
        private bool _isDestroyAllowed;

        private void Awake()
        {
            _buttons.ForEach(b => b.AddListener(OnBuildingSelected));
        }
        
        public void Initialize(GameTileContentFactory contentFactory, Camera camera, GameBoard gameBoard, bool isDestroyAllowed)
        {
            _contentFactory = contentFactory;
            _isDestroyAllowed = isDestroyAllowed;
            _camera = camera;
            _gameBoard = gameBoard;
        }

        private void Update()
        {
            if(_isEnabled == false)
                return;

            if (_pendingTile == null)
            {
                ProcessDestroying();
            }
            else
            {
                ProcessBuilding();
            }
        }

        private void ProcessBuilding()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(TouchRay, out var position))
            {
                _pendingTile.transform.position = TouchRay.GetPoint(position);
            }
            
            if (IsPointerUp())
            {
                var tile = _gameBoard.GetTile(TouchRay);
                if (tile == null || _gameBoard.TryBuild(tile, _pendingTile) == false)
                {
                    Destroy(_pendingTile.gameObject);
                }
                _pendingTile = null;
            }
        }

        private void ProcessDestroying()
        {
            if(_isDestroyAllowed == false)
                return;
            if (IsPointerUp())
            {
                var tile = _gameBoard.GetTile(TouchRay);
                if (tile != null)
                {
                    _gameBoard.DestroyTile(tile);
                }
            }
        }

        private bool IsPointerUp()
        {
            #if UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
            #else
            return Input.touches.Length == 0;
            #endif
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        private void OnBuildingSelected(GameTileContentType type)
        {
            //TODO check money
            _pendingTile = _contentFactory.Get(type);
        }
    }