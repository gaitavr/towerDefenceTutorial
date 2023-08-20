using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Core.Loading
{
    public sealed class LoginOperation : ILoadingOperation
    {
        public string Description => "Login to server...";

        private readonly UserContainer _userContainer;

        private Action<float> _onProgress;
        
        public LoginOperation(UserContainer userContainer)
        {
            _userContainer = userContainer;
        }
        
        public async UniTask Load(Action<float> onProgress)
        {
            _onProgress = onProgress;
            _onProgress?.Invoke(0.3f);

            _userContainer.State = await GetAccountState(DeviceInfoProvider.GetDeviceId());
           
            _onProgress?.Invoke(1f);
        }

        private async UniTask<UserAccountState> GetAccountState(string deviceId)
        {
            UserAccountState result = null;
            
            //Fake login
            if (PlayerPrefs.HasKey(deviceId))
            {
                result = JsonUtility.FromJson<UserAccountState>(PlayerPrefs.GetString(deviceId));
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _onProgress?.Invoke(0.6f);
            //Fake login

            if (result == null)
            {
                result = await ProjectContext.I.LoginWindowProvider.ShowAndHide();
            }
            
            PlayerPrefs.SetString(deviceId, JsonUtility.ToJson(result));
            
            return result;
        }
    }
}