using System;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class EditorHud : MonoBehaviour
    {
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _quitButton;
        
        public event Action SaveClicked;
        public event Action QuitGame;
        
        private void Awake()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        
        private async void OnQuitButtonClicked()
        {
            var isConfirmed = await AlertPopup.Instance.AwaitForDecision("Are you sure to quit?");
            if(isConfirmed)
                QuitGame?.Invoke();
        }

        private void OnSaveButtonClicked()
        {
            SaveClicked?.Invoke();
        }
    }
}