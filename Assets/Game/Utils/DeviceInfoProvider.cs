using UnityEngine;

namespace Utils
{
    public static class DeviceInfoProvider
    {
        public static string GetDeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }
}