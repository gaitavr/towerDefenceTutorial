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
        
        private void Start()
        {
            _quickGameBtn.onClick.AddListener(OnQuickGameBtnClicked);
            _editBoardBtn.onClick.AddListener(OnQuickGameBtnClicked);
        }

        private void OnQuickGameBtnClicked()
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new GameLoadingOperation());
            LoadingScreen.Instance.Load(loadingOperations);
        }
    }
}