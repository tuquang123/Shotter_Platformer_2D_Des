using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticDailyQuestData : List<StaticDailyQuestData>
{
    public StaticDailyQuestData GetData(DailyQuestType type)
    {
        for (int i = 0; i < Count; i++)
        {
            StaticDailyQuestData data = this[i];

            if (data.type == type)
            {
                return data;
            }
        }

        DebugCustom.Log("[GetStaticQuestData] Type not found=" + type);
        return null;
    }
}
