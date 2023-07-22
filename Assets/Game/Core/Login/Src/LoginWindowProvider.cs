using System.Threading.Tasks;
using AppInfo;
using Assets;
using Cysharp.Threading.Tasks;

namespace Login
{
    public class LoginWindowProvider : LocalAssetLoader
    {
        public async UniTask<UserInfoContainer> ShowAndHide()
        {
            var loginWindow = await Load();
            var result = await loginWindow.ProcessLogin();
            Unload();
            return result;
        }
    
        public UniTask<LoginWindow> Load()
        {
            return LoadInternal<LoginWindow>("LoginWindow");
        }

        public void Unload()
        {
            UnloadInternal();
        }
    }
}