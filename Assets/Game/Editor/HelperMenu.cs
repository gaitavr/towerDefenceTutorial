using Utils;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Editor
{
    public class HelperMenu
    {
        private static string UserStatePath => $"{Application.persistentDataPath}/userAccountState.def";

        [MenuItem("Tools/Progress/Clear Account Data")]
        public static void ClearAccountData()
        {
            var path = UserStatePath;
            if (File.Exists(path))
                File.Delete(path);
        }

        [MenuItem("Tools/Progress/Show user state file")]
        public static void ShowUserStateFile()
        {
            EditorUtility.RevealInFinder(UserStatePath);
        }
        
        [MenuItem("Tools/Assets/Clear Asset Bundle Cache")]
        public static void DoClearAssetBundleCache()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Debug.Log($"Clear Asset Bundle Cache result: {Caching.ClearCache()}");
        }
    }
}