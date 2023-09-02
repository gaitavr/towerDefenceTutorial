using Cysharp.Threading.Tasks;
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

        private readonly PvpGroupType _groupType;
        private readonly UserBoardState _boardState;
        private readonly UserAttackScenarioState _attackScenarioState;

        public PvpModeLoadingOperation(PvpGroupType groupType, UserBoardState boardState, UserAttackScenarioState attackScenarioState)
        {
            _groupType = groupType;
            _boardState = boardState;
            _attackScenarioState = attackScenarioState;
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

            pvpMode.Init(_groupType, _boardState, _attackScenarioState);
            onProgress?.Invoke(1.0f);
            pvpMode.BeginNewGame();
        }
    }
}
