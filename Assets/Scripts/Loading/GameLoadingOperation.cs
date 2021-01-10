using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class GameLoadingOperation : ILoadingOperation
    {
        public string GetName => "Game loading...";
        
        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await Task.Delay(10);
            }
            onProgress?.Invoke(1f);
        }
    }
}