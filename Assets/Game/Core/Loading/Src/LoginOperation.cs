using System;
using Cysharp.Threading.Tasks;
using Login;
using UnityEngine;
using Utils;
using Utils.Assets;
using System.IO;

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
            var result = new UserAccountState();

            //Fake login
            var path = $"{Application.persistentDataPath}/userAccountState.def";
            if (File.Exists(path))
            {
                var readBytes = File.ReadAllBytes(path);
                result.Deserialize(readBytes);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _onProgress?.Invoke(0.6f);
            //Fake login

            if (result.IsValid() == false)
            {
                var assetsLoader = new LocalAssetLoader();
                var loginWindow = await assetsLoader.Load<LoginWindow>(AssetsConstants.LoginWindow);
                result = await loginWindow.ProcessLogin();
                assetsLoader.Unload();

                var writeBytes = result.Serialize();
                File.WriteAllBytes(path, writeBytes);
            }

            return result;
        }
    }
}