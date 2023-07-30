using System;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class EditorHud : MonoBehaviour
    {
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _quitButton;
        
        public event Action SaveClicked;
        public event Action QuitGame;
        
        private void Awake()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        
        private async void OnQuitButtonClicked()
        {
            var popupProvider = new AlertPopupProvider();
            var popup = await popupProvider.Load();
            var isConfirmed = await popup.AwaitForDecision("Are you sure to quit?");
            if(isConfirmed)
                QuitGame?.Invoke();
            popupProvider.Unload();
        }

        private void OnSaveButtonClicked()
        {
            SaveClicked?.Invoke();
        }
    }
}