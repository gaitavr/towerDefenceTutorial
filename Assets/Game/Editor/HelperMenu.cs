using Common;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class HelperMenu
    {
        [MenuItem("Tools/Progress/Clear login")]
        public static void ClearLoginPrefs()
        {
            PlayerPrefs.DeleteKey(DeviceInfoProvider.GetDeviceId());
        }
        
        [MenuItem("Tools/Progress/Clear prefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        
        [MenuItem("Tools/Assets/Clear Asset Bundle Cache")]
        public static void DoClearAssetBundleCache()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            Debug.Log($"Clear Asset Bundle Cache result: {Caching.ClearCache()}");
        }
    }
}