using System.Collections.Generic;

public class StaticCampaignStageData
{
    public string stageNameId;
    public MapType map;
    public int[] stamina;
    public int[] coinCompleteStage;
    public Dictionary<Difficulty, List<RewardData>> firstTimeRewards;
    public Dictionary<Difficulty, List<RewardData>> rewards;
}
