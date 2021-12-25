using System;
using System.Threading.Tasks;
using AppInfo;
using Common;
using Extensions;
using Loading.Login;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loading
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
        
        public async Task Load(Action<float> onProgress)
        {
            _onProgress = onProgress;
            _onProgress?.Invoke(0.3f);
            
            _appInfoContainer.UserInfo = await GetUserInfo(DeviceInfoProvider.GetDeviceId());
           
            _onProgress?.Invoke(1f);
        }

        private async Task<UserInfoContainer> GetUserInfo(string deviceId)
        {
            UserInfoContainer result = null;
            
            //Fake login
            if (PlayerPrefs.HasKey(deviceId))
            {
                result = JsonUtility.FromJson<UserInfoContainer>(PlayerPrefs.GetString(deviceId));
            }
            await Task.Delay(TimeSpan.FromSeconds(1.5f));
            _onProgress?.Invoke(0.6f);
            //Fake login

            if (result == null)
            {
                result = await ShowLoginWindow();
            }
            
            PlayerPrefs.SetString(deviceId, JsonUtility.ToJson(result));
            
            return result;
        }

        private async Task<UserInfoContainer> ShowLoginWindow()
        {
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.LOGIN, 
                LoadSceneMode.Additive);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            
            var scene = SceneManager.GetSceneByName(Constants.Scenes.LOGIN);
            var loginWindow = scene.GetRoot<LoginWindow>();

            var result = await loginWindow.ProcessLogin();

            var unloadOp = SceneManager.UnloadSceneAsync(Constants.Scenes.LOGIN);
            while (unloadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            return result;
        }
    }
}