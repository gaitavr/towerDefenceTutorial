using System;
using Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class EditorHud : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _undoButton;
        [SerializeField] private CurrenciesUI _currenciesUI;

        public event Action SaveClicked;
        public event Action QuitGame;
        public event Action UndoAction;
        
        private void Awake()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
            _undoButton.onClick.AddListener(OnUndoButtonClicked);
            _currenciesUI.Show();
        }
        
        private async void OnQuitButtonClicked()
        {
            var popup = await AlertPopup.Load();
            var isConfirmed = await popup.Value.AwaitForDecision("Are you sure to quit?");
            popup.Dispose();
            if (isConfirmed)
                QuitGame?.Invoke();
        }

        private void OnSaveButtonClicked() => SaveClicked?.Invoke();

        private void OnUndoButtonClicked() => UndoAction?.Invoke();

        private void OnDestroy()
        {
            _currenciesUI.Hide();
        }
    }
}