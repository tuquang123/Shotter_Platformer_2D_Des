using UnityEngine;
using System.Collections;

public class SkillTreeRambo_0 : BaseSkillTree
{
    [Header("OFFENSE")]
    public S_IncreaseMeleeWeaponDamage increaseMeleeWeaponDamage;
    public S_IncreaseNormalGunDamage increaseNormalGunDamage;
    public S_IncreaseSpecialGunDamage increaseSpecialGunDamage;
    public S_GrenadeStun grenadeStun;
    public S_IncreaseDamageToUnitMuchHP increaseDamageUnitMuchHP;
    public S_ActiveBomb activeBomb;

    [Header("DEFENSE")]
    public S_IncreaseImmortalDuration increaseImmortalDuration;
    public S_IncreaseHP increaseHP;
    public S_Evade evade;
    public S_ReduceTakenDamage reduceDamageTaken;
    public S_RecoverAtLowHP recoverAtLowHP;
    public S_CreateReflectShield createReflectShield;

    [Header("UTILITY")]
    public S_BonusCoinCollect bonusCoin;
    public S_BonusExp bonusExp;
    public S_IncreaseSpeed increaseSpeed;
    public S_BonusItemHealthValue bonusItemHealthValue;
    public S_DecreaseCooldownGrenade decreaseCooldownGrenade;
    public S_Rage rage;
}
