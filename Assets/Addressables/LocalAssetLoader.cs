using System;
using System.Threading.Tasks;
using Loading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets
{
    public class LocalAssetLoader
    {
        private GameObject _cachedObject;
        
        protected async Task<T> LoadInternal<T>(string assetId)
        {
            var handle = Addressables.InstantiateAsync(assetId);
            _cachedObject = await handle.Task;
            if(_cachedObject.TryGetComponent(out T component) == false)
                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            return component;
        }

        protected void UnloadInternal()
        {
            if(_cachedObject == null)
                return;
            _cachedObject.SetActive(false);
            Addressables.ReleaseInstance(_cachedObject);
            _cachedObject = null;
        }
    }
}