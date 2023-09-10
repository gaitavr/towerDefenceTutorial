using System;
using Core.Loading;
using Cysharp.Threading.Tasks;
using GamePlay.Modes;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _pvpButton;
        [SerializeField] private Button _quickGameButton;
        [SerializeField] private Button _boardsButton;
        [SerializeField] private Button _scenarioButton;
        [SerializeField] private BoardsEditorMenu _boardsMenu;
        [SerializeField] private AttackScenarioEditorMenu _attackScenarioEditorMenu;
        [SerializeField] private PvpSelectionMenu _pvpSelectionMenu;

        private void Start()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            _pvpButton.onClick.AddListener(OnPvPButtonClicked);
            _quickGameButton.onClick.AddListener(OnQuickGameButtonnClicked);
            _boardsButton.onClick.AddListener(OnBoardsButtonClicked);
            _scenarioButton.onClick.AddListener(OnScenarioButtonClicked);
        }

        private async void OnPvPButtonClicked()
        {
            try
            {
                var groupType = await _pvpSelectionMenu.SelectGroup();
                if (groupType == PvpGroupType.Unknown)
                    return;

                switch (groupType)
                {
                    case PvpGroupType.Attack:
                    case PvpGroupType.Defend:
                        ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(
                            new PvpModeLoadingOperation(groupType, _pvpSelectionMenu.BoardState, _pvpSelectionMenu.AttackScenarioState))
                            .Forget();
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(MainMenu)} OnPvPButtonClicked exception: {e.Message}");
            }
        }

        private void OnQuickGameButtonnClicked()
        {
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new QuickGameLoadingOperation())
                .Forget();
        }

        private void OnBoardsButtonClicked()
        {
            _boardsMenu.Show();
        }

        private void OnScenarioButtonClicked()
        {
            _attackScenarioEditorMenu.Show();
        }
    }
}