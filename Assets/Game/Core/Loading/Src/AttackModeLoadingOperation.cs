using Cysharp.Threading.Tasks;
using GamePlay.Modes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Extensions;

namespace Core.Loading
{
    public sealed class AttackModeLoadingOperation : ILoadingOperation
    {
        public string Description => "Searching oponent...";

        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.3f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.ATTACK_MODE, LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await UniTask.Yield();
            }
            onProgress?.Invoke(0.7f);

            var scene = SceneManager.GetSceneByName(Constants.Scenes.ATTACK_MODE);
            var attackMode = scene.GetRoot<AttackMode>();
            var environment = await ProjectContext.I.AssetProvider.LoadSceneAdditive("Sand");

            var boardState = UserBoardState.GetInitial(new Vector2Int(5, 5), "test");//TODO search real board
            attackMode.Init(boardState, environment);
            onProgress?.Invoke(1.0f);
            attackMode.BeginNewGame();
        }
    }
}
