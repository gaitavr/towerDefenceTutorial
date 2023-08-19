using System;
using AppInfo;
using Cysharp.Threading.Tasks;
using Loading;
using UnityEngine;
using Utils;

namespace Login
{
    public class LoginOperation : ILoadingOperation
    {
        public string Description => "Login to server...";

        private readonly AppInfoContainer _appInfoContainer;

        private Action<float> _onProgress;
        
        public LoginOperation(AppInfoContainer appInfoContainer)
        {
            _appInfoContainer = appInfoContainer;
        }
        
        public async UniTask Load(Action<float> onProgress)
        {
            _onProgress = onProgress;
            _onProgress?.Invoke(0.3f);
            
            _appInfoContainer.UserInfo = await GetUserInfo(DeviceInfoProvider.GetDeviceId());
           
            _onProgress?.Invoke(1f);
        }

        private async UniTask<UserInfoContainer> GetUserInfo(string deviceId)
        {
            UserInfoContainer result = null;
            
            //Fake login
            if (PlayerPrefs.HasKey(deviceId))
            {
                result = JsonUtility.FromJson<UserInfoContainer>(PlayerPrefs.GetString(deviceId));
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