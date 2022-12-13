using UnityEngine;
using System.Collections;

public class AVM_GetRankLevel : BaseAchievement
{
    public override void Init()
    {
        base.Init();
    }

    public override void SetProgressToDefault()
    {
        int level = GameData.playerProfile.level;

        if (GameData.playerAchievements.ContainsKey(type))
        {
            GameData.playerAchievements[type].progress = level;
        }
        else
        {
            GameData.playerAchievements.Add(type, new PlayerAchievementData(type, level, 0));
        }

        progress = GameData.playerAchievements[type].progress;
    }
}