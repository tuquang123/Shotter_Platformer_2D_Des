using UnityEngine;
using System.Collections.Generic;

public class StaticAchievementData
{
    public AchievementType type;
    public string title;
    public string description;
    public List<AchievementMilestone> milestones;

    public bool isReady;
    public bool isCompleted;
}
