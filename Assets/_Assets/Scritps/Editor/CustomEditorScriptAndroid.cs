using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

#if UNITY_ANDROID && UNITY_EDITOR

[InitializeOnLoad]
public class CustomEditorScriptAndroid
{
    private static readonly string PluginName = "FCM";
    private static readonly string PLAY_SERVICES_VERSION = "latest";

    static CustomEditorScriptAndroid()
    {
        addGMSLibrary();
    }

    private static void addGMSLibrary()
    {
        // Setup the resolver using reflection as the module may not be available at compile time.
        Type playServicesSupport = Google.VersionHandler.FindClass("Google.JarResolver", "Google.JarResolver.PlayServicesSupport");

        if (playServicesSupport == null)
        {
            return;
        }

        object svcSupport = Google.VersionHandler.InvokeStaticMethod(
            playServicesSupport, "CreateInstance",
            new object[] {
                PluginName,
                EditorPrefs.GetString("AndroidSdkRoot"),
                "ProjectSettings"
            });

        Google.VersionHandler.InvokeInstanceMethod(
            svcSupport, "DependOn",
            new object[] { "com.google.android.gms", "play-services-gcm", PLAY_SERVICES_VERSION },
            namedArgs: new Dictionary<string, object>() {
                {"packageIds", new string[] {
                        "extra-google-m2repository",
                        "extra-android-m2repository"} }
            });
    }
}

#endif