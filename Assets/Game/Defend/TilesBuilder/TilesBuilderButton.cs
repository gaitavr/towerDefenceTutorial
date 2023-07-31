using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Defend.TilesBuilder
{
    [RequireComponent(typeof(Button))]
    public class TilesBuilderButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameTileContentType _tileType;

        public void Initialize(ITilesBuilder tilesBuilder)
        {
            _button.onClick.AddListener(() => tilesBuilder.SelectBuilding(_tileType));
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}