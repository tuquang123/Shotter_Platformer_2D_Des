using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticRankData : List<StaticRankData>
{
    public StaticRankData GetData(int level)
    {
        for (int i = 0; i < Count; i++)
        {
            StaticRankData data = this[i];

            if (data.level == level)
            {
                return data;
            }
        }

        DebugCustom.LogError("[GetStaticRankData] Level invalid=" + level);
        return null;
    }

    public string GetRankName(int level)
    {
        if (GameData.rankNames.ContainsKey(level))
        {
            return GameData.rankNames[level].ToUpper();
        }

        DebugCustom.LogError("[GetRankName] Rank name empty=" + level);
        return string.Empty;
    }

    public int GetExpOfLevel(int level)
    {
        for (int i = 0; i < Count; i++)
        {
            StaticRankData data = this[i];

            if (data.level == level)
            {
                return data.exp;
            }
        }

        DebugCustom.LogError("[GetExpOfLevel] Level invalid=" + level);
        return 0;
    }

    public int GetLevelByExp(int exp)
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            StaticRankData data = this[i];

            if (data.exp <= exp)
            {
                return data.level;
            }
        }

        return this[Count - 1].level;
    }
}
