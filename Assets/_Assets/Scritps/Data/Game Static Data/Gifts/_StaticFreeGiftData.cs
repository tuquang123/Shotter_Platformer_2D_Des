using UnityEngine;
using System.Collections.Generic;

public class _StaticFreeGiftData : List<List<RewardData>>
{
    public List<RewardData> GetRewards(int times)
    {
        if (times <= this.Count)
        {
            return this[times - 1];
        }
        else
        {
            DebugCustom.LogError("Invalid times get free gift=" + times);
            return this[Count - 1];
        }
    }
}
