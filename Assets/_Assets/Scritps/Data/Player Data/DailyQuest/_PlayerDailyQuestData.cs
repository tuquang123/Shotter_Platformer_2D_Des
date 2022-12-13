using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerDailyQuestData : List<PlayerDailyQuestData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerDailyQuestData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerDailyQuestData=" + s);
    }

    public int GetNumberReadyQuest()
    {
        int count = 0;

        for (int i = 0; i < Count; i++)
        {
            PlayerDailyQuestData quest = this[i];
            StaticDailyQuestData staticQuest = GameData.staticDailyQuestData.GetData(quest.type);

            if (quest.progress >= staticQuest.value && quest.isClaimed == false)
            {
                count++;
            }
        }

        return count;
    }

    public bool IsAlreadyClaimed(DailyQuestType type)
    {
        for (int i = 0; i < Count; i++)
        {
            PlayerDailyQuestData quest = this[i];

            if (quest.type == type)
            {
                return quest.isClaimed;
            }
        }

        return false;
    }
}
