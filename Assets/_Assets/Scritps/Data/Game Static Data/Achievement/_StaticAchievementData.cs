using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class _StaticAchievementData : List<StaticAchievementData>
{

    public StaticAchievementData GetData(AchievementType type)
    {
        for (int i = 0; i < Count; i++)
        {
            StaticAchievementData staticData = this[i];

            if (staticData.type == type)
            {
                return staticData;
            }
        }

        DebugCustom.Log("[GetStaticAchievementData] Type not found=" + type);
        return null;
    }

    public AchievementMilestone GetMilestone(AchievementType type, int index)
    {
        for (int i = 0; i < Count; i++)
        {
            StaticAchievementData staticData = this[i];

            if (staticData.type == type)
            {
                if (index < staticData.milestones.Count)
                {
                    return staticData.milestones[index];
                }
                else
                {
                    DebugCustom.LogError(string.Format("[GetAchievementMilestone] Type={2}, Invalid index={0}, milestones count={1}", index, staticData.milestones.Count, type));
                    return null;
                }
            }
        }

        DebugCustom.Log("[GetAchievementMilestone] Type not found=" + type);
        return null;
    }

    public void SortByState()
    {
        for (int i = 0; i < Count; i++)
        {
            StaticAchievementData staticData = this[i];

            int curMilestoneIndex = 0;

            if (GameData.playerAchievements.ContainsKey(staticData.type))
            {
                curMilestoneIndex = Mathf.Clamp(GameData.playerAchievements[staticData.type].claimTimes, 0, staticData.milestones.Count - 1);
            }

            AchievementMilestone milestone = staticData.milestones[curMilestoneIndex];

            int progress = GameData.playerAchievements.ContainsKey(staticData.type) ?
                GameData.playerAchievements[staticData.type].progress : 0;

            int target = milestone.requirement;

            staticData.isCompleted = GameData.playerAchievements.ContainsKey(staticData.type) ?
                GameData.playerAchievements[staticData.type].claimTimes >= staticData.milestones.Count : false;

            staticData.isReady = staticData.isCompleted == false && progress >= target;
        }

        List<StaticAchievementData> tmp = this.OrderBy(x => x.isCompleted).ThenByDescending(x => x.isReady).ThenBy(x => x.type).ToList();
        Clear();

        for (int i = 0; i < tmp.Count; i++)
        {
            Add(tmp[i]);
        }
    }
}
