using Utils;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Editor
{
    public class HelperMenu
    {
        [MenuItem("Tools/Progress/Clear Account Data")]
        public static void ClearAccountData()
        {
            var path = $"{Application.persistentDataPath}/userAccountState.def";
            if (File.Exists(path))
                File.Delete(path);
        }
        
        [MenuItem("Tools/Assets/Clear Asset Bundle Cache")]
        public static void DoClearAssetBundleCache()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Debug.Log($"Clear Asset Bundle Cache result: {Caching.ClearCache()}");
        }
    }
}