using System.Collections.Generic;
using Loading;
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
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new QuickGameLoadingOperation());
            LoadingScreen.Instance.Load(loadingOperations);
        }

        private void OnEditorBtnClicked()
        {
            _editorMenu.Show();
        }
    }
}