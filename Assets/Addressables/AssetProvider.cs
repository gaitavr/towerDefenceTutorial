using System;
using System.Threading.Tasks;
using Loading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AssetProvider : ILoadingOperation
{
    private bool _isReady;

    private async Task WaitUntilReady()
    {
        while (_isReady == false)
        {
            await Task.Yield();
        }
    }
    
    public async Task<SceneInstance> LoadSceneAdditive(string sceneId)
    {
        await WaitUntilReady();
        var op = Addressables.LoadSceneAsync(sceneId, 
            LoadSceneMode.Additive);
        return await op.Task;
    }

    public async Task UnloadAdditiveScene(SceneInstance scene)
    {
        await WaitUntilReady();
        var op = Addressables.UnloadSceneAsync(scene);
        await op.Task;
    }

    public string Description => "Assets Initialization...";
    public async Task Load(Action<float> onProgress)
    {
        var operation = Addressables.InitializeAsync();
        await operation.Task;
        _isReady = true;
    }
}