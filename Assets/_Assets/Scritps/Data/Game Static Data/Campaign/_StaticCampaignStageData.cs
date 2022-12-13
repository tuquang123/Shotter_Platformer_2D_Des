using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticCampaignStageData : List<StaticCampaignStageData>
{
    public int GetLevelEnemy(string id, Difficulty difficulty)
    {
        int level = 1;

        if (GameData.campaignStageLevelData.ContainsKey(id))
        {
            level = GameData.campaignStageLevelData[id];

            if (difficulty == Difficulty.Hard)
            {
                level += StaticValue.LEVEL_INCREASE_MODE_HARD;
            }
            else if (difficulty == Difficulty.Crazy)
            {
                level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
            }
        }

        level = Mathf.Clamp(level, 1, StaticValue.MAX_LEVEL_ENEMY);

        return level;
    }

    public int GetCoinDrop(string id, Difficulty difficulty)
    {
        int coin = 0;

        for (int i = 0; i < this.Count; i++)
        {
            if (string.Compare(this[i].stageNameId, id) == 0)
            {
                List<RewardData> rewards = this[i].rewards[difficulty];

                for (int j = 0; j < rewards.Count; j++)
                {
                    if (rewards[j].type == RewardType.Coin)
                    {
                        coin += rewards[j].value;
                    }
                }
            }
        }

        return coin;
    }

    public List<RewardData> GetFirstTimeRewards(string id, Difficulty difficulty)
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (string.Compare(this[i].stageNameId, id) == 0)
            {
                return this[i].firstTimeRewards[difficulty];
            }
        }

        return null;
    }

    public int GetCoinCompleteStage(string id, Difficulty difficulty)
    {
        int coin = 0;

        for (int i = 0; i < this.Count; i++)
        {
            if (string.Compare(this[i].stageNameId, id) == 0)
            {
                coin = this[i].coinCompleteStage[(int)difficulty];
            }
        }

        return coin;
    }
}
