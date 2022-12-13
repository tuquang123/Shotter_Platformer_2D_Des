using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class DailyQuestTracker : MonoBehaviour
{
    public static DailyQuestTracker Instance { get; private set; }

    public BaseDailyQuest[] questPool;
    public List<BaseDailyQuest> quests = new List<BaseDailyQuest>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            MaintainDailyQuest();
            SceneManager.sceneLoaded += OnSceneLoaded;
            EventDispatcher.Instance.RegisterListener(EventID.NewDay, (sender, param) => RefreshDailyQuest());
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

    private void Init()
    {
        DebugCustom.Log("DailyQuestTracker INIT");

        for (int i = 0; i < quests.Count; i++)
        {
            BaseDailyQuest quest = quests[i];

            if (quest.IsAlreadyClaimed() == false)
            {
                quests[i].Init();
                quests[i].SetProgressToDefault();
            }
        }

        //SetProgressToDefault();
    }

    private void SetProgressToDefault()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].SetProgressToDefault();
        }
    }

    public void Save()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].Save();
        }

        GameData.playerDailyQuests.Save();
    }

    private void MaintainDailyQuest()
    {
        if (GameData.playerDailyQuests.Count <= 0)
        {
            RefreshDailyQuest();
        }
        else
        {
            for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
            {
                PlayerDailyQuestData questData = GameData.playerDailyQuests[i];
                BaseDailyQuest questPrefab = GetQuestPrefab(questData.type);

                if (questPrefab)
                {
                    BaseDailyQuest quest = Instantiate(questPrefab, transform);
                    quest.progress = questData.progress;
                    quests.Add(quest);
                }

                DebugCustom.Log(string.Format("[Daily Quest] index={0}, id={1}, type={2}", i, (int)questData.type, questData.type));
            }
        }
    }

    private void RefreshDailyQuest()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            Destroy(quests[i].gameObject);
        }

        quests.Clear();
        GameData.playerDailyQuests.Clear();

        List<DailyQuestType> createdQuest = new List<DailyQuestType>();
        int totalDailyQuests = Enum.GetNames(typeof(DailyQuestType)).Length;
        DebugCustom.Log("Daily quest pool counts =" + totalDailyQuests);
        int typeId = 0;

        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                typeId = (int)DailyQuestType.COMPLETE_ALL_QUEST;
            }
            else if (i == 1)
            {
                typeId = (int)DailyQuestType.KILL_ENEMY;
            }
            else if (i == 2)
            {
                typeId = (int)DailyQuestType.GET_FREE_COIN;
            }
            else if (i == 3)
            {
                typeId = (int)DailyQuestType.COMPLETE_STAGE;
            }
            else if (i == 4)
            {
                //typeId = (int)DailyQuestType.PLAY_TOURNAMENT;
                typeId = (int)DailyQuestType.VIEW_ADS_X2_REWARD;
            }

            createdQuest.Add((DailyQuestType)typeId);
            BaseDailyQuest quest = Instantiate(GetQuestPrefab((DailyQuestType)typeId), transform);
            quest.name = quest.type.ToString();
            quests.Add(quest);
            GameData.playerDailyQuests.Add(new PlayerDailyQuestData(quest.type));

            DebugCustom.Log(string.Format("[NEW Daily Quest] index={0}, id={1}, type={2}", i, typeId, (DailyQuestType)typeId));
        }

        GameData.playerDailyQuests.Save();
    }

    private BaseDailyQuest GetQuestPrefab(DailyQuestType type)
    {
        for (int i = 0; i < questPool.Length; i++)
        {
            if (type == questPool[i].type)
            {
                return questPool[i];
            }
        }

        DebugCustom.Log("Quest prefab NULL=" + type);
        return null;
    }
}
