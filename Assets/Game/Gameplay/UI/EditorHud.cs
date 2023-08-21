using System;
using Utils;
using UnityEngine;
using UnityEngine.UI;
using Utils.Assets;

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
            var assetProvider = new LocalAssetLoader();
            var popup = await assetProvider.Load<AlertPopup>(AssetsConstants.AlertPopup);
            var isConfirmed = await popup.AwaitForDecision("Are you sure to quit?");
            if(isConfirmed)
                QuitGame?.Invoke();
            assetProvider.Unload();
        }

        private void OnSaveButtonClicked()
        {
            SaveClicked?.Invoke();
        }
    }
}