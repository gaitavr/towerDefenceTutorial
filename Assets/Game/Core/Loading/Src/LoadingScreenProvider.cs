using System.Collections.Generic;
using System.Threading.Tasks;
using Assets;
using Cysharp.Threading.Tasks;
using Loading;

public class LoadingScreenProvider : LocalAssetLoader
{
    public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
    {
        var loadingScreen = await Load<LoadingScreen>("LoadingScreen");
        await loadingScreen.Load(loadingOperations);
        Unload();
    }
}
