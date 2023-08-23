using System;
using Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using Utils.Serialization;
using GamePlay;

namespace Core.Loading
{
    public sealed class EditorModeLoadingOperation : ILoadingOperation
    {
        public string Description => "Board editor loading...";

        private readonly BoardContext _boardContext;

        public EditorModeLoadingOperation(BoardContext boardContext)
        {
            _boardContext = boardContext;
        }
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.3f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.EDITOR_MODE, 
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await UniTask.Delay(1);
            }
            
            var scene = SceneManager.GetSceneByName(Constants.Scenes.EDITOR_MODE);
            var editorGame = scene.GetRoot<BoardEditorMode>();
            editorGame.Init(_boardContext);
            onProgress?.Invoke(0.9f);
            
            editorGame.BeginNewGame();
            onProgress?.Invoke(1.0f);
        }
    }
}