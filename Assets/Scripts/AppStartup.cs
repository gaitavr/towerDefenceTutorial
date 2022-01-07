using System.Collections.Generic;
using AppInfo;
using Loading;
using Login;
using UnityEngine;

public class AppStartup : MonoBehaviour
{
    private LoadingScreenProvider LoadingProvider => ProjectContext.Instance.LoadingScreenProvider;
    
    private void Start()
    {
        ProjectContext.Instance.Initialize();
        
        var appInfoContainer = new AppInfoContainer();
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(ProjectContext.Instance.AssetProvider);
        loadingOperations.Enqueue(new LoginOperation(appInfoContainer));
        loadingOperations.Enqueue(new ConfigOperation(appInfoContainer));
        loadingOperations.Enqueue(new MenuLoadingOperation());

        LoadingProvider.LoadAndDestroy(loadingOperations);
    }
}