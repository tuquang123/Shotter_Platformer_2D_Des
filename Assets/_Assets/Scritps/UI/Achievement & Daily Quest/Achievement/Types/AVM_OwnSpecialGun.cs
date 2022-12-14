using UnityEngine;
using System.Collections;

public class AVM_OwnSpecialGun : BaseAchievement
{
    public override void Init()
    {
        base.Init();
    }

    public override void SetProgressToDefault()
    {
        int num = GameData.playerGuns.GetNumberOfSpecialGun();

        if (GameData.playerAchievements.ContainsKey(type))
        {
            GameData.playerAchievements[type].progress = num;
        }
        else
        {
            GameData.playerAchievements.Add(type, new PlayerAchievementData(type, num, 0));
        }

        progress = GameData.playerAchievements[type].progress;
    }

}