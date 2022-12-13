using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerCampaignStageProgressData : Dictionary<string, List<bool>>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerCampaignStageProgressData=" + s);
    }

    public List<bool> GetProgress(string stageId)
    {
        if (ContainsKey(stageId))
        {
            return this[stageId];
        }
        else
        {
            List<bool> progress = new List<bool> { false, false, false };
            return progress;
        }
    }
}
