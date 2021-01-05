using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private Slider _progressFill;
        [SerializeField]
        private TextMeshProUGUI _loadingInfo;
        [SerializeField]
        private float _barSpeed;
        
        private float _toFill;
        private Coroutine _progressBarRoutine;
        
        public async void Load(Queue<ILoadingOperation> loadingOperations, Action onComplete)
        {
            _canvas.enabled = true;
            foreach (var operation in loadingOperations)
            {
                ResetFill();
                _loadingInfo.text = operation.GetName;
                
                if(_progressBarRoutine != null)
                    StopCoroutine(_progressBarRoutine);
                _progressBarRoutine = StartCoroutine(UpdateProgressBar());
                
                await operation.Load(OnProgress);
                await WaitForBarFill();
            }
            
            StopCoroutine(_progressBarRoutine);
            
            _canvas.enabled = false;
            onComplete?.Invoke();
        }

        private void ResetFill()
        {
            _progressFill.value = 0;
            _toFill = 0;
        }

        private void OnProgress(float progress)
        {
            _toFill = progress;
        }

        private async Task WaitForBarFill()
        {
            while (_progressFill.value < _toFill)
            {
                await Task.Delay(1);
            }
            await Task.Delay(TimeSpan.FromSeconds(0.3f));
        }

        private IEnumerator UpdateProgressBar()
        {
            while (true)
            {
                if(_progressFill.value < _toFill)
                    _progressFill.value += Time.deltaTime * _barSpeed;
                yield return null;
            }
        }
    }
}