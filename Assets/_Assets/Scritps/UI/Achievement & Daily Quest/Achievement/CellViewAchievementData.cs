using System.Collections.Generic;

public class CellViewAchievementData
{
    public AchievementType type;
    public bool isCompleted;
    public string title;
    public string description;
    public int progress;
    public int target;
    public List<RewardData> rewards;
}
