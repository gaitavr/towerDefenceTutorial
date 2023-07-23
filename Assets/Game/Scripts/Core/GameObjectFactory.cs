using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameObjectFactory : ScriptableObject
{
    private Scene _scene;

    protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
    {
        if (!_scene.isLoaded)
        {
            if (Application.isEditor)
            {
                _scene = SceneManager.GetSceneByName(name);
                if (!_scene.isLoaded)
                {
                    _scene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                _scene = SceneManager.CreateScene(name);
            }
        }

        var instance = Instantiate(prefab);
        SceneManager.MoveGameObjectToScene(instance.gameObject, _scene);
        return instance;
    }

    public async UniTask Unload()
    {
        if (_scene.isLoaded)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(_scene);
            while (unloadOp.isDone == false)
            {
                await UniTask.Yield();
            }
        }
    }
}
