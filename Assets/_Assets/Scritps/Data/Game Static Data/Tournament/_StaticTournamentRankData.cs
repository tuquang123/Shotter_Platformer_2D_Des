using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticTournamentRankData : List<StaticTournamentRankData>
{
    public StaticTournamentRankData GetData(int rankIndex)
    {
        for (int i = 0; i < Count; i++)
        {
            StaticTournamentRankData data = this[i];

            if (data.rankIndex == rankIndex)
            {
                return data;
            }
        }

        DebugCustom.LogError("[GetStaticTournamentRankData] Rank index not found=" + rankIndex);
        return null;
    }

    public TournamentRank GetCurrentRank(int score)
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            StaticTournamentRankData data = this[i];

            if (data.score <= score)
            {
                return (TournamentRank)data.rankIndex;
            }
        }

        return (TournamentRank)this[Count - 1].rankIndex;
    }
}
