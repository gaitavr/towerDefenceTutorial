using Core;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Login
{
    public sealed class LoginWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameField;
        [SerializeField] private Button _facebookLogin;
        [SerializeField] private Button _simpleLogin;

        private UniTaskCompletionSource<UserAccountState> _loginCompletionSource;

        private const int NAME_MIN_LENGTH = 3;
        
        private void Awake()
        {
            _simpleLogin.onClick.AddListener(OnSimpleLoginClicked);
            _facebookLogin.onClick.AddListener(OnFacebookLoginClicked);
        }
        
        public async UniTask<UserAccountState> ProcessLogin()
        {
            _loginCompletionSource = new UniTaskCompletionSource<UserAccountState>();
            return await _loginCompletionSource.Task;
        }

        private void OnSimpleLoginClicked()
        {
            if(_nameField.text.Length < NAME_MIN_LENGTH)
                return;
            _loginCompletionSource.TrySetResult(new UserAccountState()
            {
                Name = _nameField.text
            });
        }

        private void OnFacebookLoginClicked()
        {
            //TODO implement later
        }
    }
}