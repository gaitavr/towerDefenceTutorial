using System;
using System.Threading.Tasks;
using AppInfo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Loading.Login
{
    public class LoginWindow : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _nameField;
        [SerializeField]
        private Button _facebookLogin;
        [SerializeField]
        private Button _simpleLogin;

        private TaskCompletionSource<UserInfoContainer> _loginCompletionSource;

        private const int NAME_MIN_LENGTH = 3;
        
        private void Awake()
        {
            _simpleLogin.onClick.AddListener(OnSimpleLoginClicked);
            _facebookLogin.onClick.AddListener(OnFacebookLoginClicked);
        }
        
        public async Task<UserInfoContainer> ProcessLogin()
        {
            _loginCompletionSource = new TaskCompletionSource<UserInfoContainer>();
            return await _loginCompletionSource.Task;
        }

        private void OnSimpleLoginClicked()
        {
            if(_nameField.text.Length < NAME_MIN_LENGTH)
                return;
            _loginCompletionSource.SetResult(new UserInfoContainer()
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