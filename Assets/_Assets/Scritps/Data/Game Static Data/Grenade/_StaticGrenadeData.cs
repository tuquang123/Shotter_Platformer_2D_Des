using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticGrenadeData : Dictionary<int, StaticGrenadeData>
{
    public StaticGrenadeData GetData(int id)
    {
        if (ContainsKey(id))
        {
            return this[id];
        }
        else
        {
            DebugCustom.LogError("[GetStaticGrenadeData] NULL: " + id);
            return null;
        }
    }

    public SO_GrenadeStats GetBaseStats(int id, int level)
    {
        StaticGrenadeData grenadeData = GetData(id);
        string path = string.Format(grenadeData.statsPath, level);
        return Resources.Load<SO_GrenadeStats>(path);
    }

    public float GetBattlePower(int id, int level)
    {
        float power = 0;
        SO_GrenadeStats stats = GetBaseStats(id, level);
        float damage = stats.Damage;
        float critRate = stats.CriticalRate / 100f;
        float critDamageRate = (1 + (stats.CriticalDamageBonus / 100f));

        power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * (1f / stats.Cooldown);

        return power;
    }

    public WeaponStatsGrade GetGradeDamage(float damage)
    {
        if (damage >= 100f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (damage >= 50f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (damage >= 25f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (damage >= 15f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }

    public WeaponStatsGrade GetGradeRadius(float radius)
    {
        if (radius >= 3f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (radius >= 2.5f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (radius >= 2f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (radius >= 1f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }

    public WeaponStatsGrade GetGradeCooldown(float rate)
    {
        if (rate <= 0.5f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (rate <= 1.5f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (rate <= 3f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (rate <= 4f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }
}
