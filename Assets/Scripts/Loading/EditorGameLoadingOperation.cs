using System;
using System.Threading.Tasks;
using Common;
using Extensions;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class EditorGameLoadingOperation : ILoadingOperation
    {
        public string Description => "Game loading...";

        private readonly string _fileName;

        public EditorGameLoadingOperation(string fileName)
        {
            _fileName = fileName;
        }
        
        public async Task Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.3f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.EDITOR_GAME, 
                LoadSceneMode.Single);
            while (loadOp.isDone == false)
            {
                await Task.Delay(1);
            }
            
            var scene = SceneManager.GetSceneByName(Constants.Scenes.EDITOR_GAME);
            var editorGame = scene.GetRoot<EditorGame>();
            editorGame.Init(_fileName);
            onProgress?.Invoke(0.9f);
            
            editorGame.BeginNewGame();
            onProgress?.Invoke(1.0f);
        }
    }
}