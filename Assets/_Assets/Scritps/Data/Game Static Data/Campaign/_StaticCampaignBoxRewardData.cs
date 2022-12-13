using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticCampaignBoxRewardData : List<StaticCampaignBoxRewardData>
{
    public List<RewardData> GetRewards(MapType map, int boxIndex)
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (this[i].map == map)
            {
                return this[i].rewards[boxIndex];
            }
        }

        return null;
    }
}
