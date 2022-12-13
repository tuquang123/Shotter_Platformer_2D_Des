using UnityEngine;
using System.Collections;

public class BaseDailyQuest : MonoBehaviour
{
    public DailyQuestType type;
    public int progress;

    public virtual void Init() { }

    public virtual void SetProgressToDefault()
    {
        for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
        {
            PlayerDailyQuestData quest = GameData.playerDailyQuests[i];

            if (quest.type == type)
            {
                progress = quest.progress;
                break;
            }
        }
    }

    public virtual void Save()
    {
        for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
        {
            PlayerDailyQuestData quest = GameData.playerDailyQuests[i];

            if (quest.type == type)
            {
                quest.progress = progress;
                break;
            }
        }
    }

    public virtual bool IsAlreadyClaimed()
    {
        return GameData.playerDailyQuests.IsAlreadyClaimed(type);
    }

    protected virtual void IncreaseProgress()
    {
        progress++;
    }
}
