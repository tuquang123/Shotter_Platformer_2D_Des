using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerCampaignRewardProgressData : Dictionary<MapType, List<bool>>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerCampaignRewardProgressData=" + s);
    }

    public void AddNewProgress(MapType map)
    {
        if (ContainsKey(map) == false)
        {
            List<bool> progress = new List<bool>(3);

            for (int i = 0; i < 3; i++)
            {
                progress.Add(false);
            }

            this.Add(map, progress);

            Save();
        }
    }

    public void ClaimReward(MapType map, int boxIndex)
    {
        if (ContainsKey(map))
        {
            for (int i = 0; i < this[map].Count; i++)
            {
                this[map][boxIndex] = true;
            }

            Save();
        }
        else
        {
            DebugCustom.LogError("[ClaimReward] Map not found=" + map);
        }
    }
}
