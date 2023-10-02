using System;
using Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using GamePlay.Modes;

namespace Core.Loading
{
    public sealed class DefendModeLoadingOperation : ILoadingOperation
    {
        public string Description => "Quick game loading...";
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.3f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.DEFEND_MODE, 
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await UniTask.Yield();
            }
            onProgress?.Invoke(0.7f);
            
            var scene = SceneManager.GetSceneByName(Constants.Scenes.DEFEND_MODE);
            var defendMode = scene.GetRoot<DefendMode>();
            var environment = await ProjectContext.I.AssetProvider.LoadSceneAdditive("Sand");

            var attackScenario = ProjectContext.I.UserContainer.State.AttackScenario;//TODO find
            defendMode.Init(attackScenario, environment);
            onProgress?.Invoke(1.0f);
            defendMode.BeginNewGame();
        }
    }
}