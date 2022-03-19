using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameResult
{
    [RequireComponent(typeof(Canvas))]
    public class GameResultWindow : MonoBehaviour
    {
        [SerializeField]
        private GameResultIntroAnimation _introAnimation;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _quitButton;

        private Canvas _canvas;

        private Action _onRestart;
        private Action _onQuit;
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            _restartButton.onClick.AddListener(OnRestartClicked);
            _quitButton.onClick.AddListener(OnQuitClicked);
        }
        
        public async void Show(GameResultType result, Action onRestart, Action onQuit)
        {
            _onRestart = onRestart;
            _onQuit = onQuit;
            
            _restartButton.interactable = false;
            _quitButton.interactable = false;
            
            _canvas.enabled = true;
            await _introAnimation.Play(result);

            _restartButton.interactable = true;
            _quitButton.interactable = true;
        }

        private void OnRestartClicked()
        {
            _onRestart?.Invoke();
            _canvas.enabled = false;
        }

        private void OnQuitClicked()
        {
            _onQuit?.Invoke();
            _canvas.enabled = false;
        }
    }
}
