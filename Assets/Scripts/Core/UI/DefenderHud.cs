using System;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class DefenderHud : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _wavesText;

        [SerializeField]
        private TextMeshProUGUI _playerHealthText;

        [SerializeField]
        private ToggleWithSpriteSwap _pauseToggle;

        public event Action<bool> PauseClicked;

        private void Awake()
        {
            _pauseToggle.ValueChanged += OnPauseClicked;
        }

        private void OnPauseClicked(bool isPaused)
        {
            PauseClicked?.Invoke(isPaused);
        }
        
        private void OnDestroy()
        {
            _pauseToggle.ValueChanged -= OnPauseClicked;
        }
    }
}