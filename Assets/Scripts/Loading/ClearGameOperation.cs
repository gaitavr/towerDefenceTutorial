using System;
using System.Threading.Tasks;
using Common;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class ClearGameOperation : ILoadingOperation
    {
        public string GetName => "Clearing...";

        private Game _game;

        public ClearGameOperation(Game game)
        {
            _game = game;
        }

        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.4f);
            _game.Cleanup();
            var unloadOp = SceneManager.UnloadSceneAsync(Constants.Scenes.GAME);
            while (unloadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(0.75f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, LoadSceneMode.Additive);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            onProgress?.Invoke(1f);
        }
    }
}