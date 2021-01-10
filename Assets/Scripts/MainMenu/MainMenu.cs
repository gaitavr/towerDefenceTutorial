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
        
        private void Start()
        {
            _quickGameBtn.onClick.AddListener(OnQuickGameBtnClicked);
        }

        private void OnQuickGameBtnClicked()
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new GameLoadingOperation());
            LoadingScreen.Instance.Load(loadingOperations);
        }
    }
}