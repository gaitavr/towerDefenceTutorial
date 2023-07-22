using System;
using System.Threading.Tasks;
using Common;
using Cysharp.Threading.Tasks;
using Extensions;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class QuickGameLoadingOperation : ILoadingOperation
    {
        public string Description => "Game loading...";
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.QUICK_GAME, 
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await UniTask.Delay(1);
            }
            onProgress?.Invoke(0.7f);
            
            var scene = SceneManager.GetSceneByName(Constants.Scenes.QUICK_GAME);
            var editorGame = scene.GetRoot<QuickGame>();
            var environment = await ProjectContext.Instance.AssetProvider.LoadSceneAdditive("Sand");
            onProgress?.Invoke(0.85f);
            editorGame.Init(environment);
            editorGame.BeginNewGame();
            onProgress?.Invoke(1.0f);
        }
    }
}