using UnityEngine;
using System.Collections;

public class PlayerDailyQuestData
{
    public DailyQuestType type;
    public int progress;
    public bool isClaimed;

    public PlayerDailyQuestData(DailyQuestType type, int progress = 0, bool isClaimed = false)
    {
        this.type = type;
        this.progress = progress;
        this.isClaimed = isClaimed;
    }
}
