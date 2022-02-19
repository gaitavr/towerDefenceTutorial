using System.Collections;
using TMPro;
using UnityEngine;

namespace Core.Pause
{
    public class PauseHint : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _hint;
        [SerializeField]
        private float _delay;
        [SerializeField]
        private float _time;

        private bool _inProcess;

        private void Awake()
        {
            _hint.alpha = 0;
            ProjectContext.Instance.PauseManager.SetHint(this);
        }
        
        public void TryShow()
        {
            if(_inProcess)
                return;

            StartCoroutine(ShowAndHide());
        }
        
        private IEnumerator ShowAndHide()
        {
            _inProcess = true;
            _hint.alpha = 0;
            
            var t = _time;
            while (t > 0)
            {
                _hint.alpha += Time.deltaTime / _time; 
                t -= Time.deltaTime;
                yield return null;
            }

            _hint.alpha = 1;
            yield return new WaitForSeconds(_delay);
            
            t = _time;
            while (t > 0)
            {
                _hint.alpha -= Time.deltaTime / _time; 
                t -= Time.deltaTime;
                yield return null;
            }
            
            _hint.alpha = 0;
            _inProcess = false;
        }
    }
}