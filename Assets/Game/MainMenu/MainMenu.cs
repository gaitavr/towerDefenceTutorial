using System.Collections.Generic;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Gameplay;
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
        [SerializeField] private Button _scenariosButton;
        [SerializeField] private BoardsEditorMenu _boardsMenu;

        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        private void Start()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            _pvpButton.onClick.AddListener(OnPvPButtonClicked);
            _quickGameButton.onClick.AddListener(OnQuickGameButtonnClicked);
            _boardsButton.onClick.AddListener(OnBoardsButtonClicked);
            _scenariosButton.onClick.AddListener(OnScenariosButtonClicked);
        }

        private void OnPvPButtonClicked()
        {
            var boardContext = new BoardContext(UserState.Boards[0]);//TODO temporary
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new PvpModeLoadingOperation(boardContext))
                .Forget();
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

        private void OnScenariosButtonClicked()
        {
            //Show scenario edit window
        }
    }
}