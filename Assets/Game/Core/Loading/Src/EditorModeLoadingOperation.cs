using System;
using Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace Core.Loading
{
    public sealed class EditorModeLoadingOperation : ILoadingOperation
    {
        public string Description => "Game loading...";

        private readonly string _fileName;

        public EditorModeLoadingOperation(string fileName)
        {
            _fileName = fileName;
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
            editorGame.Init(new UnityEngine.Vector2Int(10, 15));
            onProgress?.Invoke(0.9f);
            
            editorGame.BeginNewGame();
            onProgress?.Invoke(1.0f);
        }
    }
}