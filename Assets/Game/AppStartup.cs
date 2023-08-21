using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using Core.Loading;
using UnityEngine;

public class AppStartup : MonoBehaviour
{
    private LoadingScreenProvider LoadingProvider => ProjectContext.I.LoadingScreenProvider;
    
    private void Start()
    {
        ProjectContext.I.Initialize();
        
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(ProjectContext.I.AssetProvider);
        loadingOperations.Enqueue(new LoginOperation());
        loadingOperations.Enqueue(new ConfigOperation());
        loadingOperations.Enqueue(new MenuLoadingOperation());

        LoadingProvider.LoadAndDestroy(loadingOperations).Forget();
    }
}