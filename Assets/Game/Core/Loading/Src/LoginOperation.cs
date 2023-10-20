using System;
using Cysharp.Threading.Tasks;
using Login;
using Utils.Assets;
using Utils.Extensions;

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

            ProjectContext.I.UserContainer.State = await GetAccountState();
           
            _onProgress?.Invoke(1f);
        }

        private async UniTask<UserAccountState> GetAccountState()
        {
            var userStateCommunicator = ProjectContext.I.UserStateCommunicator;
            var result = await userStateCommunicator.GetUserState();
            _onProgress?.Invoke(0.6f);

            if (result.IsValid() == false)
            {
                var assetsLoader = new LocalAssetLoader();
                var loginWindow = await assetsLoader.Load<LoginWindow>(AssetsConstants.LoginWindow);
                result = await loginWindow.ProcessLogin();
                assetsLoader.Unload();

                await userStateCommunicator.SaveUserState(result);
            }

            return result;
        }
    }
}