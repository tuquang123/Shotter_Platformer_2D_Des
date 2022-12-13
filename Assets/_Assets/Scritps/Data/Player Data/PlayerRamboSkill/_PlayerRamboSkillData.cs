using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerRamboSkillData : Dictionary<int, PlayerRamboSkillData> // key = ramboID
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerRamboSkillData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerRamboSkillData=" + s);
    }

    public PlayerRamboSkillData GetRamboSkillProgress(int ramboId)
    {
        if (this.ContainsKey(ramboId))
        {
            return this[ramboId];
        }
        else
        {
            DebugCustom.LogError("[GetRamboSkillProgress] Rambo id not found=" + ramboId);
            return null;
        }
    }

    public int GetUsedSkillPoints(int ramboId)
    {
        int points = 0;

        PlayerRamboSkillData progress = GetRamboSkillProgress(ramboId);

        foreach (KeyValuePair<int, int> skill in progress)
        {
            if (skill.Value > 0)
            {
                points++;
            }
        }

        return points;
    }

    public int GetUnusedSkillPoints(int ramboId)
    {
        int points = 0;

        int usedPoints = GetUsedSkillPoints(ramboId);
        int level = GameData.playerRambos.GetRamboLevel(ramboId);

        points = Mathf.Clamp(level - 1 - usedPoints, 0, level - 1);

        return points;
    }
}
