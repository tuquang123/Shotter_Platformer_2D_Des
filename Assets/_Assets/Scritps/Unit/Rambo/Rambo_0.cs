using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class Rambo_0 : Rambo
{
    [Header("RAMBO JOHN")]
    public GameObject effectRage;
    [SpineAnimation]
    public string rage;
    [SpineSkin]
    public string skinDefault, skinRage;

    public SkillTreeRambo_0 Skills { get { return (SkillTreeRambo_0)skillTree; } }

    private bool isReadyRegen = true;
    private int cooldownRegen = 60;

    private bool isReadyReflect = true;
    private bool isReflect;
    private int cooldownReflect = 60;

    public BombSupportSkill bombPrefab;
    private bool isReadyBomb = true;
    private int cooldownBomb = 60;
    private WaitForSeconds bombInterval = new WaitForSeconds(0.05f);

    private bool isReadyRage = true;
    private bool isRage;
    private int cooldownRage = 60;


    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.ActiveReflectShield, (sender, param) => ActiveReflectShield());
        EventDispatcher.Instance.RegisterListener(EventID.ActiveBomb, (sender, param) => ActiveBomb());
        EventDispatcher.Instance.RegisterListener(EventID.ActiveRage, (sender, param) => ActiveRage());
    }

    public override void Renew()
    {
        base.Renew();

        isReflect = false;
        effectRage.SetActive(false);
        ActiveSkinRage(false);
    }

    public override AttackData GetMeleeWeaponAttackData()
    {
        AttackData atkData = base.GetMeleeWeaponAttackData();

        if (Skills.increaseMeleeWeaponDamage.level > 0)
        {
            float damage = atkData.damage * (1 + (Skills.increaseMeleeWeaponDamage.value / 100f));
            atkData.damage = damage;
        }

        return atkData;
    }

    public override AttackData GetGunAttackData()
    {
        AttackData atkData = base.GetGunAttackData();

        if (atkData.weapon == WeaponType.NormalGun)
        {
            if (Skills.increaseNormalGunDamage.level > 0)
            {
                float damage = atkData.damage * (1 + (Skills.increaseNormalGunDamage.value / 100f));
                atkData.damage = damage;
            }
        }
        else if (atkData.weapon == WeaponType.SpecialGun)
        {
            if (Skills.increaseSpecialGunDamage.level > 0)
            {
                float damage = atkData.damage * (1 + (Skills.increaseSpecialGunDamage.value / 100f));
                atkData.damage = damage;
            }
        }

        if (Skills.increaseDamageUnitMuchHP.level > 0)
        {
            DebuffData debuff = new DebuffData(DebuffType.TakeMoreDamageWhenHighHP, 0f, Skills.increaseDamageUnitMuchHP.value);

            if (atkData.debuffs == null)
            {
                List<DebuffData> debuffs = new List<DebuffData>();
                debuffs.Add(debuff);
                atkData.debuffs = debuffs;
            }
            else
            {
                atkData.debuffs.Add(debuff);
            }
        }

        return atkData;
    }

    public override AttackData GetGrenadeAttackData(BaseGrenade grenade)
    {
        AttackData atkData = base.GetGrenadeAttackData(grenade);

        if (Skills.grenadeStun.level > 0)
        {
            DebuffData debuff = new DebuffData(DebuffType.Stun, Skills.grenadeStun.value);

            if (atkData.debuffs == null)
            {
                List<DebuffData> debuffs = new List<DebuffData>();
                debuffs.Add(debuff);
                atkData.debuffs = debuffs;
            }
            else
            {
                atkData.debuffs.Add(debuff);
            }
        }

        return atkData;
    }

    protected override float GetReviveImmortalDuration()
    {
        if (Skills.increaseImmortalDuration.level > 0)
        {
            return Skills.increaseImmortalDuration.value;
        }
        else
        {
            return base.GetReviveImmortalDuration();
        }
    }

    protected override void CalculateBaseStatsIncrease()
    {
        base.CalculateBaseStatsIncrease();

        if (Skills.increaseHP.level > 0)
        {
            float hpIncrease = baseStats.HP * (Skills.increaseHP.value / 100f);
            stats.AdjustStats(hpIncrease, StatsType.MaxHp);
        }

        if (Skills.increaseSpeed.level > 0)
        {
            float speedIncrease = baseStats.MoveSpeed * (Skills.increaseSpeed.value / 100f);
            stats.AdjustStats(speedIncrease, StatsType.MoveSpeed);
        }
    }

    public override void TakeDamage(AttackData attackData)
    {
        if (isDead || attackData.attacker.isDead)
            return;

        if (isImmortal)
        {
            EffectController.Instance.SpawnTextTMP(bodyCenterPoint.position, Color.yellow, "BLOCK", parent: PoolingController.Instance.groupText);

            if (isReflect)
            {
                float reflectDamage = attackData.damage * 0.15f;
                DebuffData debuff = new DebuffData(DebuffType.Reflect, 0f, reflectDamage);
                List<DebuffData> debuffs = new List<DebuffData>() { debuff };
                AttackData atkData = new AttackData(this, reflectDamage, debuffs: debuffs);
                attackData.attacker.TakeDamage(atkData);
            }
        }
        else
        {
            if (Skills.evade.level > 0)
            {
                bool isEvade = Random.Range(1, 101) <= Skills.evade.value;

                if (isEvade)
                {
                    EffectController.Instance.SpawnTextTMP(bodyCenterPoint.position, Color.gray, "EVADE", parent: PoolingController.Instance.groupText);
                    return;
                }
            }

            if (Skills.reduceDamageTaken.level > 0)
            {
                float damageReduce = attackData.damage * Mathf.Clamp01(Skills.reduceDamageTaken.value / 100f);
                attackData.damage -= damageReduce;
            }

            EffectTakeDamage();
            ShowTextDamageTaken(attackData.damage);
            stats.AdjustStats(-attackData.damage, StatsType.Hp);
            UpdateHealthBar();

            if (HpPercent < 0.2f)
                ActiveSoundLowHp(true);

            if (stats.HP <= 0)
            {
                Die();

                if (GameData.mode == GameMode.Campaign)
                {
                    if (((BaseEnemy)attackData.attacker).isFinalBoss)
                    {
                        FirebaseAnalyticsHelper.LogEvent("N_KilledByFinalBoss",
                            string.Format("ID={0},{1}-{2}", ((BaseEnemy)attackData.attacker).id, GameData.currentStage.id, GameData.currentStage.difficulty));
                    }
                }
            }
            else if (HpPercent <= 0.2)
            {
                if (Skills.recoverAtLowHP.level > 0)
                {
                    if (isReadyRegen)
                    {
                        isReadyRegen = false;
                        StartCoroutine(CoroutineRegen());
                    }
                }
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isDead == false)
        {
            if (isImmortal)
            {
                EffectController.Instance.SpawnTextTMP(bodyCenterPoint.position, Color.yellow, "BLOCK", parent: PoolingController.Instance.groupText);
            }
            else
            {
                if (Skills.evade.level > 0)
                {
                    bool isEvade = Random.Range(1, 101) <= Skills.evade.value;

                    if (isEvade)
                    {
                        EffectController.Instance.SpawnTextTMP(bodyCenterPoint.position, Color.gray, "EVADE", parent: PoolingController.Instance.groupText);
                        return;
                    }
                }

                if (Skills.reduceDamageTaken.level > 0)
                {
                    float damageReduce = damage * Mathf.Clamp01(Skills.reduceDamageTaken.value / 100f);
                    damage -= damageReduce;
                }

                EffectTakeDamage();
                ShowTextDamageTaken(damage);
                stats.AdjustStats(-damage, StatsType.Hp);
                UpdateHealthBar();

                if (stats.HP <= 0)
                {
                    Die();
                }
                else if (HpPercent <= 0.2)
                {
                    if (Skills.recoverAtLowHP.level > 0)
                    {
                        if (isReadyRegen)
                        {
                            isReadyRegen = false;
                            StartCoroutine(CoroutineRegen());
                        }
                    }
                }
            }
        }
    }

    protected override void OnFinishStage(float delayEndGame)
    {
        isImmortal = true;

        this.StartDelayAction(() =>
        {
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_VOICE_VICTORY);
            PlayAnimationVictory();

            if (Skills.bonusCoin.level > 0)
            {
                EventDispatcher.Instance.PostEvent(EventID.BonusCoinCollected, Skills.bonusCoin.value);
            }

            if (Skills.bonusExp.level > 0)
            {
                EventDispatcher.Instance.PostEvent(EventID.BonusExpWin, Skills.bonusExp.value);
            }

        }, delayEndGame);
    }

    protected override void RestoreHP(float value, bool isFromItemDrop)
    {
        if (isFromItemDrop)
        {
            float hpRestore = (stats.MaxHp - stats.HP) * 0.5f;

            if (hpRestore < 500)
                hpRestore = 500;

            if (Skills.bonusItemHealthValue.level > 0)
            {
                hpRestore *= (1 + (Skills.bonusItemHealthValue.value / 100f));
            }

            stats.AdjustStats(hpRestore, StatsType.Hp);

            if (stats.HP > stats.MaxHp)
            {
                stats.SetStats(stats.MaxHp, StatsType.Hp);
            }

            UpdateHealthBar();
            ActiveSoundLowHp(HpPercent < 0.2f);
            effectRestoreHP.Play();
            int hpDisplay = Mathf.RoundToInt(hpRestore * 10f);
            EffectController.Instance.SpawnTextTMP(BodyCenterPoint.position, Color.green, string.Format("+{0} HP", hpDisplay), parent: PoolingController.Instance.groupText);
            SoundManager.Instance.PlaySfx(soundRevive);
        }
        else
        {
            base.RestoreHP(value, false);
        }
    }

    protected override IEnumerator CoroutineCooldownGrenade(float cooldown)
    {
        float newCooldown = cooldown;

        if (Skills.decreaseCooldownGrenade.level > 0)
        {
            newCooldown = cooldown * (1 - (Skills.decreaseCooldownGrenade.value / 100f));
        }

        isCooldownGrenade = true;
        UIController.Instance.SetCooldownButtonGrenade(false);
        float count = 0;

        while (isCooldownGrenade)
        {
            count += Time.deltaTime;
            isCooldownGrenade = count < newCooldown;
            float percentCooldown = Mathf.Clamp01(count / newCooldown);
            UIController.Instance.imageCooldownGrenade.fillAmount = percentCooldown;
            UIController.Instance.textCooldownGrenade.text = string.Format("{0:f1}", newCooldown - count);
            yield return null;
        }

        isCooldownGrenade = false;
        UIController.Instance.SetCooldownButtonGrenade(true);
    }

    private IEnumerator CoroutineRegen()
    {
        int timer = 0;
        float hp = stats.MaxHp * (Skills.recoverAtLowHP.value / 100f);

        while (timer < 5)
        {
            RestoreHP(hp, isFromItemDrop: false);
            timer++;
            yield return StaticValue.waitOneSec;
        }

        StartCoroutine(CoroutineCooldownRegen());
    }

    private IEnumerator CoroutineCooldownRegen()
    {
        int timer = 0;

        while (timer < cooldownRegen)
        {
            timer++;
            yield return StaticValue.waitOneSec;
        }

        isReadyRegen = true;
    }


    #region Skill Defense - Reflect Shield
    private void ActiveReflectShield()
    {
        if (isReadyReflect)
        {
            isReadyReflect = false;
            StartCoroutine(CoroutineReflect());
            StartCoroutine(CoroutineCooldownReflect());
        }
    }

    private IEnumerator CoroutineReflect()
    {
        isReflect = true;
        isImmortal = true;
        effectImmortal.SetActive(true);
        int duration = (int)Skills.createReflectShield.value;

        while (duration > 0)
        {
            duration--;
            yield return StaticValue.waitOneSec;
        }

        isReflect = false;
        isImmortal = false;
        effectImmortal.SetActive(false);
    }

    private IEnumerator CoroutineCooldownReflect()
    {
        int timer = 0;
        UIController.Instance.EnableSkill(false);

        while (timer < cooldownReflect)
        {
            float percent = Mathf.Clamp01((float)timer / (float)cooldownReflect);
            UIController.Instance.SetCooldownSkill(percent);
            UIController.Instance.SetTextCooldownSkill(cooldownReflect - timer);
            timer++;
            yield return StaticValue.waitOneSec;
        }

        UIController.Instance.EnableSkill(true);
        isReadyReflect = true;
    }
    #endregion

    #region Skill Offense - Bomb
    private void ActiveBomb()
    {
        if (isReadyBomb)
        {
            isReadyBomb = false;
            StartCoroutine(CoroutineBomb());
            StartCoroutine(CoroutineCooldownBomb());
        }
    }

    private void ReleaseBomb()
    {
        BombSupportSkill bomb = PoolingController.Instance.poolBombSupportSkill.New();

        if (bomb == null)
        {
            bomb = Instantiate<BombSupportSkill>(bombPrefab);
        }

        Vector2 v = CameraFollow.Instance.top.position;
        v.y += 1.5f;
        v.x = Random.Range(CameraFollow.Instance.left.position.x, CameraFollow.Instance.right.position.x);

        bomb.Active(v, Random.Range(50f, 80f));
    }

    private IEnumerator CoroutineBomb()
    {
        float duration = Skills.activeBomb.value;

        while (duration > 0)
        {
            duration -= 0.3f;
            ReleaseBomb();
            yield return bombInterval;
        }
    }

    private IEnumerator CoroutineCooldownBomb()
    {
        int timer = 0;
        UIController.Instance.EnableSkill(false);

        while (timer < cooldownBomb)
        {
            float percent = Mathf.Clamp01((float)timer / (float)cooldownBomb);
            UIController.Instance.SetCooldownSkill(percent);
            UIController.Instance.SetTextCooldownSkill(cooldownBomb - timer);
            timer++;
            yield return StaticValue.waitOneSec;
        }

        UIController.Instance.EnableSkill(true);
        isReadyBomb = true;
    }
    #endregion

    #region Skill Utility - Rage
    private void ActiveRage()
    {
        if (isReadyRage)
        {
            isReadyRage = false;
            StartCoroutine(CoroutineRage());
            StartCoroutine(CoroutineCooldownRage());
        }
    }

    private void IncreaseStats(float percent)
    {
        AddModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, percent));
        AddModifier(new ModifierData(StatsType.AttackTimePerSecond, ModifierType.AddPercentBase, percent));
        AddModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, percent));

        ReloadStats();
    }

    private void RemoveStatsBonus(float percent)
    {
        RemoveModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, percent));
        RemoveModifier(new ModifierData(StatsType.AttackTimePerSecond, ModifierType.AddPercentBase, percent));
        RemoveModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, percent));

        ReloadStats();
    }

    private IEnumerator CoroutineRage()
    {
        isRage = true;
        effectRage.SetActive(false);
        effectRage.SetActive(true);
        ActiveSkinRage(true);
        int duration = 15;

        float value = Skills.rage.value / 100f;
        IncreaseStats(value);

        while (duration > 0 && isDead == false)
        {
            duration--;
            yield return StaticValue.waitOneSec;
        }

        isRage = false;
        effectRage.SetActive(false);
        ActiveSkinRage(false);
        RemoveStatsBonus(value);
    }

    private IEnumerator CoroutineCooldownRage()
    {
        int timer = 0;
        UIController.Instance.EnableSkill(false);

        while (timer < cooldownRage)
        {
            float percent = Mathf.Clamp01((float)timer / (float)cooldownRage);
            UIController.Instance.SetCooldownSkill(percent);
            UIController.Instance.SetTextCooldownSkill(cooldownRage - timer);
            timer++;
            yield return StaticValue.waitOneSec;
        }

        UIController.Instance.EnableSkill(true);
        isReadyRage = true;
    }

    private void ActiveSkinRage(bool isActive)
    {
        string skin = isActive ? skinRage : skinDefault;
        skeletonAnimation.Skeleton.SetSkin(skin);
    }
    #endregion
}
