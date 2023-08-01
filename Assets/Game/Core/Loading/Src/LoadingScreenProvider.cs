using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loading;
using Utils.Assets;

public class LoadingScreenProvider : LocalAssetLoader
{
    public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
    {
        var loadingScreen = await Load<LoadingScreen>(AssetsConstants.LoadingScreen);
        await loadingScreen.Load(loadingOperations);
        Unload();
    }
}
