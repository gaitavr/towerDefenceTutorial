using Core;
using Cysharp.Threading.Tasks;
using Utils.Assets;

namespace Login
{
    public sealed class LoginWindowProvider : LocalAssetLoader
    {
        public async UniTask<UserAccountState> ShowAndHide()
        {
            var loginWindow = await Load<LoginWindow>(AssetsConstants.LoginWindow);
            var result = await loginWindow.ProcessLogin();
            Unload();
            return result;
        }
    }
}