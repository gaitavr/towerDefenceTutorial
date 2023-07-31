using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets
{
    public class LocalAssetLoader
    {
        private GameObject _cachedObject;
        
        public async UniTask<T> Load<T>(string assetId, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(assetId, parent);
            _cachedObject = await handle.Task;
            if(_cachedObject.TryGetComponent(out T component) == false)
                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            return component;
        }

        public void Unload()
        {
            if(_cachedObject == null)
                return;
            _cachedObject.SetActive(false);
            Addressables.ReleaseInstance(_cachedObject);
            _cachedObject = null;
        }
    }
}