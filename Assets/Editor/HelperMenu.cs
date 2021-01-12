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
    }
}