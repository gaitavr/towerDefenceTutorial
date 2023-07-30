using System;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class DefenderHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wavesText;
        [SerializeField] private TextMeshProUGUI _playerHealthText;
        [SerializeField] private ToggleWithSpriteSwap _pauseToggle;
        [SerializeField] private Button _quitButton;
        public event Action QuitGame;

        private void Awake()
        {
            _pauseToggle.ValueChanged += OnPauseClicked;
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        public void UpdatePlayerHealth(float currentHp, float maxHp)
        {
            _playerHealthText.text = $"{(int)(currentHp / maxHp * 100)}%";
        }

        public void UpdateScenarioWaves(int currentWave, int wavesCount)
        {
            _wavesText.text = $"{currentWave}/{wavesCount}";
        }

        private async void OnQuitButtonClicked()
        {
            OnPauseClicked(true);
            var popupProvider = new AlertPopupProvider();
            var popup = await popupProvider.Load();
            var isConfirmed = await popup.AwaitForDecision("Are you sure to quit?");
            OnPauseClicked(false);
            if(isConfirmed)
                QuitGame?.Invoke();
            popupProvider.Unload();
        }
        
        private void OnPauseClicked(bool isPaused)
        {
            ProjectContext.I.PauseManager.SetPaused(isPaused);
        }
        
        private void OnDestroy()
        {
            _pauseToggle.ValueChanged -= OnPauseClicked;
        }
    }
}