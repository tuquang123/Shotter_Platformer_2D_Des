using System.Collections.Generic;

public class CellViewDailyQuestData
{
    public DailyQuestType type;
    public string title;
    public string description;
    public bool isClaimed;
    public int progress;
    public int target;
    public List<RewardData> rewards;
}
