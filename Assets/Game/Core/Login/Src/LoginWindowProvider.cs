using AppInfo;
using Cysharp.Threading.Tasks;
using Utils.Assets;

namespace Login
{
    public class LoginWindowProvider : LocalAssetLoader
    {
        public async UniTask<UserInfoContainer> ShowAndHide()
        {
            var loginWindow = await Load<LoginWindow>(AssetsConstants.LoginWindow);
            var result = await loginWindow.ProcessLogin();
            Unload();
            return result;
        }
    }
}