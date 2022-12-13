using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerCampaignProgressData : Dictionary<Difficulty, PlayerCampaignProgressData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerCampaignProgessData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerCampaignProgress=" + s);
    }
}
