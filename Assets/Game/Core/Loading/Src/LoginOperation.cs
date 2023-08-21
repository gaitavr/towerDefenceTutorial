using System;
using Cysharp.Threading.Tasks;
using Login;
using UnityEngine;
using Utils;
using Utils.Assets;

namespace Core.Loading
{
    public sealed class LoginOperation : ILoadingOperation
    {
        public string Description => "Login to server...";

        private Action<float> _onProgress;
        
        public async UniTask Load(Action<float> onProgress)
        {
            _onProgress = onProgress;
            _onProgress?.Invoke(0.3f);

            ProjectContext.I.UserContainer.State = await GetAccountState(DeviceInfoProvider.GetDeviceId());
           
            _onProgress?.Invoke(1f);
        }

        private async UniTask<UserAccountState> GetAccountState(string deviceId)
        {
            UserAccountState result = null;
            
            //Fake login
            if (PlayerPrefs.HasKey(deviceId))
                result = JsonUtility.FromJson<UserAccountState>(PlayerPrefs.GetString(deviceId));

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _onProgress?.Invoke(0.6f);
            //Fake login

            if (result == null)
            {
                var assetsLoader = new LocalAssetLoader();
                var loginWindow = await assetsLoader.Load<LoginWindow>(AssetsConstants.LoginWindow);
                result = await loginWindow.ProcessLogin();
                assetsLoader.Unload();
            }

            PlayerPrefs.SetString(deviceId, JsonUtility.ToJson(result));
            
            return result;
        }
    }
}