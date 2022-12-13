using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitStats
{
    public float Damage { get { return damage; } }
    public float HP { get { return hp; } }
    public float MaxHp { get { return maxHp; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float AttackTimePerSecond { get { return attackTimePerSecond; } }
    public float AttackRate { get { return 1f / attackTimePerSecond; } }
    public float CriticalRate { get { return criticalRate; } }
    public float CriticalDamageBonus { get { return criticalDamageBonus; } }


    [SerializeField]
    private float damage;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float attackTimePerSecond;
    [SerializeField]
    private float criticalRate;
    [SerializeField]
    private float criticalDamageBonus;

    private SO_BaseUnitStats baseStats;

    public void Init(SO_BaseUnitStats baseStats)
    {
        this.baseStats = baseStats;
        ResetToBaseStats();
        hp = maxHp;
    }

    public void ResetToBaseStats()
    {
        maxHp = baseStats.HP;
        damage = baseStats.Damage;
        attackTimePerSecond = baseStats.AttackTimePerSecond;
        criticalRate = baseStats.CriticalRate;
        criticalDamageBonus = baseStats.CriticalDamageBonus;
        moveSpeed = baseStats.MoveSpeed;
    }

    public void AdjustStats(float value, StatsType type)
    {
        switch (type)
        {
            case StatsType.AttackTimePerSecond:
                attackTimePerSecond += value;

                if (attackTimePerSecond <= 0)
                    attackTimePerSecond = 0;

                break;

            case StatsType.Damage:
                damage += value;

                if (damage <= 0)
                    damage = 0;

                break;

            case StatsType.Hp:
                hp += value;

                if (hp <= 0)
                    hp = 0;

                break;

            case StatsType.MaxHp:
                maxHp += value;

                if (maxHp <= 0)
                    maxHp = 0;

                break;

            case StatsType.MoveSpeed:
                moveSpeed += value;

                if (moveSpeed <= 0.2f)
                    moveSpeed = 0.2f;

                break;

            case StatsType.CriticalRate:
                criticalRate += value;

                if (criticalRate <= 0)
                    criticalRate = 0;

                break;

            case StatsType.CriticalDamageBonus:
                criticalDamageBonus += value;

                if (criticalDamageBonus <= 0)
                    criticalDamageBonus = 0;

                break;
        }
    }

    public void SetStats(float value, StatsType type)
    {
        switch (type)
        {
            case StatsType.AttackTimePerSecond:
                attackTimePerSecond = value;
                break;

            case StatsType.Damage:
                damage = value;
                break;

            case StatsType.Hp:
                hp = value;
                break;

            case StatsType.MoveSpeed:
                moveSpeed = value;
                break;

            case StatsType.CriticalRate:
                moveSpeed = value;
                break;

            case StatsType.CriticalDamageBonus:
                moveSpeed = value;
                break;
        }
    }
}
