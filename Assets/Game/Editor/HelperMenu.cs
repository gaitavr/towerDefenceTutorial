using Utils;
using UnityEditor;
using UnityEngine;
using System.IO;
using Core;

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

        [MenuItem("Tools/Progress/Add Currencies")]
        public static void AddCurrencies()
        {
            var path = UserStatePath;
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var state = new UserAccountState();
                state.Deserialize(bytes);
                state.Currencies.ChangeCrystals(100000);
                state.Currencies.ChangeGas(1000);
                bytes = state.Serialize();
                File.WriteAllBytes(path, bytes);
            }
        }
        
        [MenuItem("Tools/Assets/Clear Asset Bundle Cache")]
        public static void DoClearAssetBundleCache()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Debug.Log($"Clear Asset Bundle Cache result: {Caching.ClearCache()}");
        }
    }
}