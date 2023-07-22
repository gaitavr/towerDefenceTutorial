using UnityEngine;

public static class DeviceInfoProvider
{
    public static string GetDeviceId()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
}