using System.Collections.Generic;
using AppInfo;
using Loading;
using UnityEngine;

public class AppStartup : MonoBehaviour
{
    private LoadingScreenProvider LoadingProvider => ProjectContext.Instance.LoadingScreenProvider;
    
    private async void Start()
    {
        ProjectContext.Instance.Initialize();
        
        var appInfoContainer = new AppInfoContainer();
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new LoginOperation(appInfoContainer));
        loadingOperations.Enqueue(new ConfigOperation(appInfoContainer));
        loadingOperations.Enqueue(new MenuLoadingOperation());

        var loadingScreen = await LoadingProvider.Load();
        await loadingScreen.Load(loadingOperations);
        LoadingProvider.Unload();
    }
}