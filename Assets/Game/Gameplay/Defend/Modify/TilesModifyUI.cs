using System.Collections.Generic;
using UnityEngine;

namespace Game.Defend.Tiles
{
    public class TilesModifyUI : MonoBehaviour
    {
        [SerializeField] private TilesModifyButton[] _buttons;

        public IEnumerable<TilesModifyButton> Buttons => _buttons;
    }
}