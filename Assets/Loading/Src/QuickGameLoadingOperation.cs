using System;
using System.Threading.Tasks;
using Common;
using Extensions;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class QuickGameLoadingOperation : ILoadingOperation
    {
        public string Description => "Game loading...";
        
        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.QUICK_GAME, 
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            var scene = SceneManager.GetSceneByName(Constants.Scenes.QUICK_GAME);
            var editorGame = scene.GetRoot<QuickGame>();
            editorGame.Init();
            onProgress?.Invoke(0.9f);
            
            editorGame.BeginNewGame();
            onProgress?.Invoke(1.0f);
        }
    }
}