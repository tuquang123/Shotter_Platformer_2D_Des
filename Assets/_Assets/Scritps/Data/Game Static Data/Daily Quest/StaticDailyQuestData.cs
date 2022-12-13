using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticDailyQuestData
{
    public DailyQuestType type;
    public int value;
    public string title;
    public string description;
    public int skipPrice;
    public List<RewardData> rewards;
}
