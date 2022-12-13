using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticCampaignBoxRewardData
{
    public MapType map;
    public Dictionary<int, List<RewardData>> rewards; //Key=boxIndex
}
