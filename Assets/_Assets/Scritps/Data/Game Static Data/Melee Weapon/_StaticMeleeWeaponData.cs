using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticMeleeWeaponData : Dictionary<int, StaticMeleeWeaponData>
{
    public StaticMeleeWeaponData GetData(int id)
    {
        if (ContainsKey(id))
        {
            return this[id];
        }
        else
        {
            DebugCustom.LogError("[GetStaticMeleeWeaponData] NULL: " + id);
            return null;
        }
    }

    public SO_MeleeWeaponStats GetBaseStats(int id, int level)
    {
        StaticMeleeWeaponData weaponData = GetData(id);
        string path = string.Format(weaponData.statsPath, level);
        return Resources.Load<SO_MeleeWeaponStats>(path);
    }

    public float GetBattlePower(int id, int level)
    {
        float power = 0;
        SO_MeleeWeaponStats stats = GetBaseStats(id, level);
        float damage = stats.Damage;
        float critRate = stats.CriticalRate / 100f;
        float critDamageRate = (1 + (stats.CriticalDamageBonus / 100f));

        power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * stats.AttackTimePerSecond;

        return power;
    }

    public WeaponStatsGrade GetGradeDamage(float damage)
    {
        if (damage >= 100f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (damage >= 70f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (damage >= 50f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (damage >= 30f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }

    public WeaponStatsGrade GetGradeAttackSpeed(float atkSpeed)
    {
        if (atkSpeed >= 2f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (atkSpeed >= 1f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (atkSpeed >= 0.5f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (atkSpeed >= 0.25f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }

    public WeaponStatsGrade GetGradeCritRate(float rate)
    {
        if (rate >= 50f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (rate >= 35f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (rate >= 15f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (rate >= 5f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }

    public WeaponStatsGrade GetGradeCritDamage(float damage)
    {
        if (damage > 100f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (damage >= 80f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (damage >= 60f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (damage >= 50f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }
}
