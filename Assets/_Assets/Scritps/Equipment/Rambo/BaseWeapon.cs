using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : BaseEquipment
{
    public AudioClip[] attackSounds;

    protected float damage;
    protected float attackTimePerSecond;
    protected float criticalRate;
    protected float criticalDamageBonus;


    public override void Init(int level)
    {
        this.level = level;

        if (isLoadedScriptableObject == false)
        {
            isLoadedScriptableObject = true;
            LoadScriptableObject();
        }
    }

    public override void ApplyOptions(BaseUnit unit)
    {
        Init(level);

        unit.stats.AdjustStats(damage, StatsType.Damage);
        unit.stats.AdjustStats(attackTimePerSecond, StatsType.AttackTimePerSecond);
        unit.stats.AdjustStats(criticalRate, StatsType.CriticalRate);
        unit.stats.AdjustStats(criticalDamageBonus, StatsType.CriticalDamageBonus);
    }

    public abstract void Attack(AttackData attackData);

    public virtual void PlaySoundAttack()
    {
        if (attackSounds.Length > 0)
        {
            int index = Random.Range(0, attackSounds.Length);
            SoundManager.Instance.PlaySfx(attackSounds[index]);
        }
    }
}
