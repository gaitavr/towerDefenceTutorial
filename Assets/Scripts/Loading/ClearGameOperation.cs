using System;
using System.Threading.Tasks;
using Common;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class ClearGameOperation : ILoadingOperation
    {
        public string Description => "Clearing...";

        private readonly QuickGame _quickGame;

        public ClearGameOperation(QuickGame quickGame)
        {
            _quickGame = quickGame;
        }

        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.2f);
            _quickGame.Cleanup();

            foreach (var factory in _quickGame.Factories)
            {
                await factory.Unload();
            }
            onProgress?.Invoke(0.5f);
            
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, LoadSceneMode.Additive);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(0.75f);
           
            var unloadOp = SceneManager.UnloadSceneAsync(Constants.Scenes.QUICK_GAME);
            while (unloadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(1f);
        }
    }
}