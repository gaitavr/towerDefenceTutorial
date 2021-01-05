using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class MenuLoadingOperation : ILoadingOperation
    {
        public string GetName => "Main menu loading...";
        
        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            while (loadOp.isDone)
            {
                await Task.Delay(10);
            }
            onProgress?.Invoke(1f);
        }
    }
}