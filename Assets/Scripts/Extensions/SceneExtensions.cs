using UnityEngine;
using UnityEngine.SceneManagement;

namespace Extensions
{
    public static class SceneExtensions
    {
        public static T GetRoot<T>(this Scene scene) where T : MonoBehaviour
        {
            var rootObjects = scene.GetRootGameObjects();
            
            T result = default;
            foreach (var go in rootObjects)
            {
                if (go.TryGetComponent(out result))
                {
                    break;
                }
            }

            return result;
        }
    }
}