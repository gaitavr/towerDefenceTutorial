using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Assets;
using Utils.Disposable;

namespace Utils
{
    [RequireComponent(typeof(Canvas))]
    public class AlertPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _closeButton;

        private UniTaskCompletionSource<bool> _taskCompletion;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            _okButton.onClick.AddListener(OnAccept);
            _cancelButton.onClick.AddListener(OnCancelled);
            _closeButton.onClick.AddListener(OnCancelled);
        }

        public async UniTask<bool> AwaitForDecision(string text)
        {
            _text.text = text;
            _canvas.enabled = true;
            _taskCompletion = new UniTaskCompletionSource<bool>();
            var result = await _taskCompletion.Task;
            _canvas.enabled = false;
            return result;
        }

        public static UniTask<Disposable<AlertPopup>> Load()
        {
            var assetLoader = new LocalAssetLoader();
            return assetLoader.LoadDisposable<AlertPopup>(AssetsConstants.AlertPopup);
        }
        
        private void OnAccept()
        {
            _taskCompletion.TrySetResult(true);
        }

        private void OnCancelled()
        {
            _taskCompletion.TrySetResult(false);
        }
    }
}