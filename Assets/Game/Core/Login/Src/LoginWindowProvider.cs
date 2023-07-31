using AppInfo;
using Assets;
using Cysharp.Threading.Tasks;

namespace Login
{
    public class LoginWindowProvider : LocalAssetLoader
    {
        public async UniTask<UserInfoContainer> ShowAndHide()
        {
            var loginWindow = await Load<LoginWindow>("LoginWindow");
            var result = await loginWindow.ProcessLogin();
            Unload();
            return result;
        }
    }
}