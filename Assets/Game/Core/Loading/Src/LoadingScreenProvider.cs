using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Utils.Assets;

namespace Core.Loading
{
    public sealed class LoadingScreenProvider : LocalAssetLoader
    {
        public async UniTask LoadAndDestroy(ILoadingOperation loadingOperation)
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(loadingOperation);
            await LoadAndDestroy(operations);
        }

        public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
        {
            var loadingScreen = await Load<LoadingScreen>(AssetsConstants.LoadingScreen);
            await loadingScreen.Load(loadingOperations);
            Unload();
        }
    }
}
