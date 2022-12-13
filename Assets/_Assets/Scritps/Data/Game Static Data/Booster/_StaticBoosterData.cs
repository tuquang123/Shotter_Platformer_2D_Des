using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticBoosterData : Dictionary<BoosterType, StaticBoosterData>
{
    public StaticBoosterData GetData(BoosterType type)
    {
        if (ContainsKey(type))
        {
            return this[type];
        }
        else
        {
            DebugCustom.LogError("[GetStaticBoosterData] NULL: " + type);
            return null;
        }
    }
}
