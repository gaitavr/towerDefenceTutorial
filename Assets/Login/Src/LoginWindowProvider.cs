using System.Threading.Tasks;
using AppInfo;
using Assets;

namespace Login
{
    public class LoginWindowProvider : LocalAssetLoader
    {
        public async Task<UserInfoContainer> ShowAndHide()
        {
            var loginWindow = await Load();
            var result = await loginWindow.ProcessLogin();
            Unload();
            return result;
        }
    
        public Task<LoginWindow> Load()
        {
            return LoadInternal<LoginWindow>("LoginWindow");
        }

        public void Unload()
        {
            UnloadInternal();
        }
    }
}