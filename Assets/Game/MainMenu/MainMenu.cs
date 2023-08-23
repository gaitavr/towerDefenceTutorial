using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _quickGameBtn;
        [SerializeField] private Button _editBoardBtn;
        [SerializeField] private BoardsEditorMenu _editorMenu;
        
        private void Start()
        {
            _quickGameBtn.onClick.AddListener(OnQuickGameBtnClicked);
            _editBoardBtn.onClick.AddListener(OnEditorBtnClicked);
        }

        private void OnQuickGameBtnClicked()
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(new QuickGameLoadingOperation());
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(operations).Forget();
        }

        private void OnEditorBtnClicked()
        {
            _editorMenu.Show();
        }
    }
}