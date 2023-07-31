using System.Collections.Generic;
using UnityEngine;

namespace Game.Defend.TilesBuilder
{
    public class TilesBuilderUI : MonoBehaviour
    {
        [SerializeField] private TilesBuilderButton[] _buttons;

        public IEnumerable<TilesBuilderButton> Buttons => _buttons;
    }
}