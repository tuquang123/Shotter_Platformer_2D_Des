using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class _PlayerAchievementData : Dictionary<AchievementType, PlayerAchievementData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerAchievementData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerAchievementData=" + s);
    }

    public int GetNumberReadyAchievement()
    {
        int count = 0;

        foreach (PlayerAchievementData achievement in Values)
        {
            StaticAchievementData staticData = GameData.staticAchievementData.GetData(achievement.type);

            if (achievement.claimTimes < staticData.milestones.Count)
            {
                AchievementMilestone milestone = staticData.milestones[achievement.claimTimes];

                if (achievement.progress >= milestone.requirement)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public bool IsAlreadyCompleted(AchievementType type)
    {
        StaticAchievementData staticData = GameData.staticAchievementData.GetData(type);

        bool b = ContainsKey(type) ? this[type].claimTimes >= staticData.milestones.Count : false;

        return b;
    }
}
