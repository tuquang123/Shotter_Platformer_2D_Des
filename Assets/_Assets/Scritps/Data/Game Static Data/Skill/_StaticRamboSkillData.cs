using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticRamboSkillData : List<StaticRamboSkillData>
{
    public StaticRamboSkillData GetData(int skillId)
    {
        for (int i = 0; i < this.Count; i++)
        {
            StaticRamboSkillData data = this[i];

            if (data.id == skillId)
            {
                return data;
            }
        }

        DebugCustom.LogError("[GetStaticRamboSkillData] Skill not found id=" + skillId);
        return null;
    }
}
