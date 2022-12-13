using Newtonsoft.Json;
using UnityEngine;

public class Utility
{
    public static string GetDeviceId()
    {
        string deviceId = string.Empty;

#if UNITY_EDITOR
        deviceId = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_ANDROID
        AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityPlayerActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityPlayerResolver = unityPlayerActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass androidSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        deviceId = androidSettingsSecure.CallStatic<string>("getString", unityPlayerResolver, "android_id");

#elif UNITY_IOS
        string keyChainJsonData = KeyChain.BindGetKeyChainUser();
        KeyChainData keyChain = JsonConvert.DeserializeObject<KeyChainData>(keyChainJsonData);
        deviceId = keyChain.uuid;

        Debug.Log(keyChainJsonData);
        Debug.Log(keyChain.ToString());

        if (string.IsNullOrEmpty(deviceId))
        {
            deviceId = SystemInfo.deviceUniqueIdentifier;
            KeyChain.BindSetKeyChainUser("com.sevenapp.metalblackops", deviceId);
        }
#else 
        deviceId = SystemInfo.deviceUniqueIdentifier;
#endif

        return deviceId;
    }

    public static void OpenStore()
    {
        Application.OpenURL(StaticValue.storeUrl);
    }
}
