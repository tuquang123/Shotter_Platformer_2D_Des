using UnityEngine;
using System.Collections;

public class BaseAchievement : MonoBehaviour
{
    public AchievementType type;
    public int progress;

    public virtual void Init() { }

    public virtual void SetProgressToDefault()
    {
        if (GameData.playerAchievements.ContainsKey(type))
        {
            progress = GameData.playerAchievements[type].progress;
        }
        else
        {
            progress = 0;
        }
    }

    public virtual void Save()
    {
        if (GameData.playerAchievements.ContainsKey(type))
        {
            GameData.playerAchievements[type].progress = progress;
        }
        else
        {
            GameData.playerAchievements.Add(type, new PlayerAchievementData(type, progress));
        }
    }

    public virtual bool IsAlreadyCompleted()
    {
        return GameData.playerAchievements.IsAlreadyCompleted(type);
    }

    protected virtual void IncreaseProgress()
    {
        progress++;
    }
}
