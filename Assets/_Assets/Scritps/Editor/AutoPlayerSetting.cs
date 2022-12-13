using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoPlayerSetting
{
    static AutoPlayerSetting()
    {
#if UNITY_ANDROID
        PlayerSettings.productName = "Metal Black OPS";
        PlayerSettings.Android.keystoreName = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "rambo.keystore";
        PlayerSettings.Android.keystorePass = "1234569";
        PlayerSettings.Android.keyaliasName = "rambo";
        PlayerSettings.Android.keyaliasPass = "1234569";
#elif UNITY_IOS
        PlayerSettings.productName = "Metal Black OPS";
#endif
    }
}
