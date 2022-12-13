using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AchievementTracker : MonoBehaviour
{
    public static AchievementTracker Instance { get; private set; }

    public BaseAchievement[] achievementPrefabs;
    public List<BaseAchievement> achievements = new List<BaseAchievement>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            CreateAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    private void CreateAchievements()
    {
        achievements.Clear();

        for (int i = 0; i < achievementPrefabs.Length; i++)
        {
            BaseAchievement achievement = Instantiate(achievementPrefabs[i], transform);
            achievement.name = achievement.type.ToString();
            achievement.SetProgressToDefault();
            achievements.Add(achievement);
        }
    }

    private void Init()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            BaseAchievement achievement = achievements[i];

            if (achievement.IsAlreadyCompleted() == false)
            {
                achievement.Init();
                achievement.SetProgressToDefault();
            }
        }

        //SetProgressToDefault();
    }

    public void SetProgressToDefault()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            achievements[i].SetProgressToDefault();
        }
    }

    public void Save()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            achievements[i].Save();
        }

        GameData.playerAchievements.Save();
    }
}
