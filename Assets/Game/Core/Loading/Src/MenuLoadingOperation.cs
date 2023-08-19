using System;
using Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class MenuLoadingOperation : ILoadingOperation
    {
        public string Description => "Main menu loading...";
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, 
                LoadSceneMode.Additive);
            while (loadOp.isDone == false)
            {
                await UniTask.Yield();
            }
            onProgress?.Invoke(1f);
        }
    }
}