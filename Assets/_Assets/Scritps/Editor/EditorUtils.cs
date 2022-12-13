using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorUtils : MonoBehaviour
{
    [MenuItem("Editor Utils/Open Scene/Root &1")]
    public static void OpenSceneRoot()
    {
        OpenScene("Root");
    }

    [MenuItem("Editor Utils/Open Scene/Login &2")]
    public static void OpenSceneLogin()
    {
        OpenScene("Login");
    }

    [MenuItem("Editor Utils/Open Scene/Menu &3")]
    public static void OpenSceneMenu()
    {
        OpenScene("Menu");
    }

    [MenuItem("Editor Utils/Open Scene/Game Play &4")]
    public static void OpenSceneGamePlay()
    {
        OpenScene("GamePlay");
    }

    [MenuItem("Editor Utils/Open Scene/Test Map &5")]
    public static void OpenSceneTestMap()
    {
        OpenScene("TestMap");
    }

    [MenuItem("Editor Utils/Clear Data")]
    public static void ClearData()
    {
        Popup.Instance.setting.ResetData();
    }

    [MenuItem("Editor Utils/Cheat Data")]
    public static void CheatPlayerData()
    {
        Popup.Instance.setting.MaxData();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 1")]
    public static void TestDailyGiftDay1()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(1);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 2")]
    public static void TestDailyGiftDay2()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(2);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 3")]
    public static void TestDailyGiftDay3()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(3);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 4")]
    public static void TestDailyGiftDay4()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(4);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 5")]
    public static void TestDailyGiftDay5()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(5);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 6")]
    public static void TestDailyGiftDay6()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(6);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Day 7")]
    public static void TestDailyGiftDay7()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(7);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/Daily Gift/Reset")]
    public static void Reset()
    {
        ProfileManager.UserProfile.getDailyGiftDay.Set(1);
        ProfileManager.UserProfile.isPassFirstWeek.Set(false);
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
        ProfileManager.SaveAll();
    }

    [MenuItem("Editor Utils/New Day")]
    public static void NewDay()
    {
        ProfileManager.UserProfile.dateLastLogin.Set(StaticValue.defaultDate);
        ProfileManager.SaveAll();
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/_Assets/Scenes/" + sceneName + ".unity");
        }
    }
}