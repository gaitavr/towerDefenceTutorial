using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Button))]
    public class ToggleWithSpriteSwap : MonoBehaviour
    {
        [SerializeField]
        private Sprite _offSprite;
        [SerializeField]
        private Sprite _onSprite;
        [SerializeField]
        private Image _changableImage;
        
        private Button _button;

        private bool _isOn;
        
        public event Action<bool> ValueChanged;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _isOn = false;
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _isOn = !_isOn;
            _changableImage.sprite = _isOn ? _onSprite : _offSprite;
            ValueChanged?.Invoke(_isOn);
        }
    }
}