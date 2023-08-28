using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Defend
{
    [RequireComponent(typeof(Button))]
    public class TilesModifyButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TileModifyActions _actionType;

        public void Initialize(ITilesModifier tilesModifier)
        {
            _button.onClick.AddListener(() => tilesModifier.DoWithTile(_actionType));
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}