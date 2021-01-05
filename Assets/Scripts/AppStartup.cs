using System.Collections.Generic;
using Loading;
using UnityEngine;

public class AppStartup : MonoBehaviour
{
    [SerializeField]
    private LoadingScreen loadingScreen;

    private void Start()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new DeviceInfoOperation());
        loadingOperations.Enqueue(new UpdateOperation());
        loadingOperations.Enqueue(new MenuLoadingOperation());
        loadingScreen.Load(loadingOperations, OnLoadingFinished);
    }

    private void OnLoadingFinished()
    {
        Debug.Log("Startup loading has been finished");
    }
}