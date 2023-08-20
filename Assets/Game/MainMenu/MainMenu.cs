using System.Collections.Generic;
using Core.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button _quickGameBtn;
        [SerializeField]
        private Button _editBoardBtn;
        [SerializeField]
        private EditorMenu _editorMenu;
        
        private void Start()
        {
            _quickGameBtn.onClick.AddListener(OnQuickGameBtnClicked);
            _editBoardBtn.onClick.AddListener(OnEditorBtnClicked);
        }

        private void OnQuickGameBtnClicked()
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(new QuickGameLoadingOperation());
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(operations);
        }

        private void OnEditorBtnClicked()
        {
            _editorMenu.Show();
        }
    }
}