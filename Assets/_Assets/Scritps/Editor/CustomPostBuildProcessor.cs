#if UNITY_IOS && UNITY_5
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class CustomPostBuildProcessor : MonoBehaviour
{
#if UNITY_CLOUD_BUILD
	public static void OnPostprocessBuildiOS (string exportPath)
	{
		Debug.Log("OnPostprocessBuildiOS");
		ProcessPostBuild(BuildTarget.iPhone,exportPath);
	}
#endif

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        //if (buildTarget != BuildTarget.iPhone) { // For Unity < 5
        if (buildTarget != BuildTarget.iOS)
        {
            Debug.LogWarning("Target is not iOS. PostProcess will not run");
            return;
        }

#if !UNITY_CLOUD_BUILD
        Debug.Log("OnPostprocessBuild");
        ProcessPostBuild(buildTarget, path);
#endif
    }

    private static void ProcessPostBuild(BuildTarget buildTarget, string path)
    {
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        string target = proj.TargetGuidByName("Unity-iPhone");

        // Add -ObjC to "Other Linker Flags"
        proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");

        // Enable Bitcode OFF
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");

        // Enable Module Objective C and C++
        proj.SetBuildProperty(target, "CLANG_ENABLE_MODULES", "YES");

		// Add GoogleService-Info.plist to Xcode project (Firebase plugin error not add plist file to Xcode project automatically)
		if (!File.Exists(path + "/GoogleService-Info.plist"))
		{
            if (File.Exists("Assets/GoogleService-Info.plist"))
            {
                FileUtil.CopyFileOrDirectory("Assets/GoogleService-Info.plist", path + "/GoogleService-Info.plist");
            }
            else
            {
                Debug.Log("Project is using Firebase but missing 'Assets/GoogleServices-Info.plist'. Please download from Firebase Console and save to Asset folder");
            }
		}

		string guid = proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist");
		proj.AddFileToBuild(target, guid);


        File.WriteAllText(projPath, proj.WriteToString());
    }
}
#endif