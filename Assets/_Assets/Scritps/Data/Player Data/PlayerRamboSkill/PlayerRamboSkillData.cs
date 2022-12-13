using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRamboSkillData : Dictionary<int, int> // Dictionary<skillID, skillLevel>
{
    public int GetSkillLevel(int skillId)
    {
        if (this.ContainsKey(skillId))
        {
            return this[skillId];
        }
        else
        {
            DebugCustom.LogError("[GetSkillLevel] Skill id not found=" + skillId);
            return 0;
        }
    }

    public void IncreaseLevel(int skillId)
    {
        if (this.ContainsKey(skillId))
        {
            this[skillId]++;

            GameData.playerRamboSkills.Save();
        }
        else
        {
            DebugCustom.LogError("[IncreaseLevel] Skill id not found=" + skillId);
        }
    }

    public void Reset()
    {
        List<int> keys = new List<int>(this.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            int key = keys[i];
            this[key] = 0;
        }

        GameData.playerRamboSkills.Save();
    }
}
