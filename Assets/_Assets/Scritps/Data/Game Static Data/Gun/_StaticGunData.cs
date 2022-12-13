using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticGunData : Dictionary<int, StaticGunData>
{
    public StaticGunData GetData(int id)
    {
        if (ContainsKey(id))
        {
            return this[id];
        }
        else
        {
            DebugCustom.LogError("[GetStaticGunData] NULL: " + id);
            return null;
        }
    }

    public SO_GunStats GetBaseStats(int id, int level)
    {
        StaticGunData gunData = GetData(id);
        string path = string.Format(gunData.statsPath, level);
        return Resources.Load<SO_GunStats>(path);
    }

    public float GetFireRate(int id, int level)
    {
        float fireRate = 0;

        SO_GunStats stats = GetBaseStats(id, level);

        if (id == StaticValue.GUN_ID_FLAME)
        {
            fireRate = 1f / ((SO_GunFlameStats)stats).TimeApplyDamage;
        }
        else if (id == StaticValue.GUN_ID_LASER)
        {
            fireRate = 1f / ((SO_GunLaserStats)stats).TimeApplyDamage;
        }
        else if (id == StaticValue.GUN_ID_TESLA)
        {
            fireRate = 1f / ((SO_GunTeslaStats)stats).TimeApplyDamage;
        }
        else
        {
            return stats.AttackTimePerSecond;
        }

        return fireRate;
    }

    public float GetBattlePower(int id, int level)
    {
        float power = 0;
        SO_GunStats stats = GetBaseStats(id, level);
        float damage = stats.Damage;
        float critRate = stats.CriticalRate / 100f;
        float critDamageRate = (1 + (stats.CriticalDamageBonus / 100f));

        if (id == StaticValue.GUN_ID_KAME_POWER)
        {
            damage *= 0.75f;
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * stats.AttackTimePerSecond * (1 / ((SO_GunKamePowerStats)stats).ChargeTime);
        }
        else if (id == StaticValue.GUN_ID_FLAME)
        {
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * (1 / ((SO_GunFlameStats)stats).TimeApplyDamage);
        }
        else if (id == StaticValue.GUN_ID_LASER)
        {
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * (1 / ((SO_GunLaserStats)stats).TimeApplyDamage);
        }
        else if (id == StaticValue.GUN_ID_FIRE_BALL)
        {
            power = ((1 - critRate) * (damage * (2f / stats.BulletSpeed) / ((SO_GunFireBallStats)stats).TimeApplyDamage) + (critRate * critDamageRate * damage * (2f / stats.BulletSpeed) / ((SO_GunFireBallStats)stats).TimeApplyDamage))
                * stats.AttackTimePerSecond;
        }
        else if (id == StaticValue.GUN_ID_SPREAD)
        {
            damage *= 3;
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * stats.AttackTimePerSecond;
        }
        else if (id == StaticValue.GUN_ID_TESLA)
        {
            damage *= 1.3f;
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * (1 / ((SO_GunTeslaStats)stats).TimeApplyDamage);
        }
        else if (id == StaticValue.GUN_ID_ROCKET_CHASER)
        {
            damage *= 1.2f;
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * stats.AttackTimePerSecond;
        }
        else
        {
            power = ((1 - critRate) * damage + critRate * critDamageRate * damage) * stats.AttackTimePerSecond;
        }

        return power;
    }

    public WeaponStatsGrade GetGradeDamage(float damage)
    {
        if (damage >= 40f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (damage >= 25f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (damage >= 10f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (damage >= 3.5f)
        {
            return WeaponStatsGrade.Grade_B;
        }
        else
        {
            return WeaponStatsGrade.Grade_C;
        }
    }

    public WeaponStatsGrade GetGradeFireRate(float rate)
    {
        if (rate >= 6.5f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (rate >= 5f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (rate >= 2.2f)
        {
            return WeaponStatsGrade.Grade_A;
        }
        else if (rate >= 1f)
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
        if (rate >= 20f)
        {
            return WeaponStatsGrade.Grade_SS;
        }
        else if (rate >= 15f)
        {
            return WeaponStatsGrade.Grade_S;
        }
        else if (rate >= 10f)
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
