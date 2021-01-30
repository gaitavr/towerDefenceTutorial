using System;
using System.Threading.Tasks;
using Common;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class ClearGameOperation : ILoadingOperation
    {
        public string Description => "Clearing...";

        private readonly Game _game;

        public ClearGameOperation(Game game)
        {
            _game = game;
        }

        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.2f);
            _game.Cleanup();

            foreach (var factory in _game.Factories)
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
           
            var unloadOp = SceneManager.UnloadSceneAsync(Constants.Scenes.GAME);
            while (unloadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(1f);
        }
    }
}