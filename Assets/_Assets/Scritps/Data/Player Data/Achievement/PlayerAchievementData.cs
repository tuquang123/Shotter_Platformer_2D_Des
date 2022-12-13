using UnityEngine;
using System.Collections;

public class PlayerAchievementData
{
    public AchievementType type;
    public int claimTimes;
    public int progress;
    public bool isReady;

    public PlayerAchievementData(AchievementType type, int progress, int claimTimes = 0, bool isReady = false)
    {
        this.type = type;
        this.claimTimes = claimTimes;
        this.progress = progress;
        this.isReady = isReady;
    }
}
