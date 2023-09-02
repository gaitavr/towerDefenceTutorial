using Cysharp.Threading.Tasks;
using Gameplay;
using GamePlay.Modes;
using System;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Extensions;

namespace Core.Loading
{
    public sealed class PvpModeLoadingOperation : ILoadingOperation
    {
        public string Description => "Searching oponent...";

        private readonly BoardContext _boardContext;

        public PvpModeLoadingOperation(BoardContext boardContext)
        {
            _boardContext = boardContext;
        }

        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.3f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.PVP_MODE, LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await UniTask.Yield();
            }
            onProgress?.Invoke(0.7f);

            var scene = SceneManager.GetSceneByName(Constants.Scenes.PVP_MODE);
            var pvpMode = scene.GetRoot<PvpMode>();

            pvpMode.Init(_boardContext, UserAttackScenarioState.GetInitial("test"));
            onProgress?.Invoke(1.0f);
            pvpMode.BeginNewGame();
        }
    }
}
