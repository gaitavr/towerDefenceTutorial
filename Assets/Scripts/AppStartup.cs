using System.Collections.Generic;
using AppInfo;
using Loading;
using UnityEngine;

public class AppStartup : MonoBehaviour
{
    private void Start()
    {
        var appInfoContainer = new AppInfoContainer();
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new LoginOperation(appInfoContainer));
        loadingOperations.Enqueue(new ConfigOperation(appInfoContainer));
        loadingOperations.Enqueue(new MenuLoadingOperation());
        LoadingScreen.Instance.Load(loadingOperations);
    }
}