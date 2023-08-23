using System;
using Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using GamePlay;

namespace Core.Loading
{
    public sealed class QuickGameLoadingOperation : ILoadingOperation
    {
        public string Description => "Game loading...";
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.QUICK_GAME_MODE, 
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await UniTask.Yield();
            }
            onProgress?.Invoke(0.7f);
            
            var scene = SceneManager.GetSceneByName(Constants.Scenes.QUICK_GAME_MODE);
            var editorGame = scene.GetRoot<QuickGameMode>();
            var environment = await ProjectContext.I.AssetProvider.LoadSceneAdditive("Sand");
            onProgress?.Invoke(0.85f);
            editorGame.Init(environment);
            editorGame.BeginNewGame();
            onProgress?.Invoke(1.0f);
        }
    }
}