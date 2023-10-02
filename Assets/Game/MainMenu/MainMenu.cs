using System;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _defendButton;
        [SerializeField] private Button _boardsButton;
        [SerializeField] private BoardsEditorMenu _boardsMenu;
        [SerializeField] private AttackScenarioEditorMenu _attackScenarioEditorMenu;
        
        private void Start()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            _attackButton.onClick.AddListener(OnAttackButtonClicked);
            _defendButton.onClick.AddListener(OnDefendButtonClicked);
            _boardsButton.onClick.AddListener(OnBoardsButtonClicked);
        }

        private async void OnAttackButtonClicked()
        {
            try
            {
                await _attackScenarioEditorMenu.Show();
                ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(
                        new AttackModeLoadingOperation())
                    .Forget();
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(MainMenu)} {nameof(OnAttackButtonClicked)} exception: {e.Message}");
            }
        }

        private void OnDefendButtonClicked()
        {
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new DefendModeLoadingOperation())
                .Forget();
        }

        private void OnBoardsButtonClicked()
        {
            _boardsMenu.Show();
        }
    }
}