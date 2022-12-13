using UnityEngine;
using System.Collections;
using Spine.Unity;

public class BaseMeleeWeapon : BaseWeapon
{
    [Header("BASE STATS")]
    public SO_MeleeWeaponStats baseStats;
    public WindBlade windEffect;

    [Header("MELEE WEAPON PROPERTIES")]
    public MeleeWeaponType type;
    public BaseEffect effectTextPrefab;

    public override void LoadScriptableObject() { }

    public override void Init(int level)
    {
        base.Init(level);

        damage = baseStats.Damage;
        attackTimePerSecond = baseStats.AttackTimePerSecond;
        criticalRate = baseStats.CriticalRate;
        criticalDamageBonus = baseStats.CriticalDamageBonus;
    }

    public override void Attack(AttackData attackData) { }

    public virtual void ActiveEffect(bool isActive)
    {
        if (windEffect)
            windEffect.Active(isActive);
    }

    public virtual void InitEffect(SkeletonAnimation skeleton, string boneName)
    {
        if (windEffect)
        {
            BoneFollower bone = windEffect.gameObject.AddComponent<BoneFollower>();
            bone.skeletonRenderer = skeleton;
            bone.boneName = boneName;
            windEffect.transform.parent = null;
        }
    }

    public virtual void SpawnEffectText(Vector2 position, Transform parent = null)
    {
        EffectTextWHAM effect = PoolingController.Instance.poolTextWHAM.New();

        if (effect == null)
        {
            effect = Instantiate(effectTextPrefab) as EffectTextWHAM;
        }

        effect.Active(position, parent);
    }
}
