using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerRamboData : Dictionary<int, PlayerRamboData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerRamboData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerRamboData=" + s);
    }

    public int GetRamboLevel(int id)
    {
        int level = 1;

        if (ContainsKey(id))
        {
            level = this[id].level;
        }
        else
        {
            DebugCustom.LogError("[GetRamboLevel] Key not found=" + id);
        }

        return level;
    }

    public void IncreaseRamboLevel(int id)
    {
        if (ContainsKey(id))
        {
            PlayerRamboData rambo = this[id];
            rambo.level++;
            Save();
        }
        else
        {
            DebugCustom.LogError("[IncreaseRamboLevel] Key not found=" + id);
        }
    }
}
