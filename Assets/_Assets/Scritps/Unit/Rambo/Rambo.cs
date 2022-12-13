using CnControls;
using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class Rambo : BaseUnit
{
    [Header("TRANSFORM")]
    public Transform spawnCrossBulletPoint;
    public Transform throwGrenadePoint;
    public Transform groundCheck;
    public Transform groupWeapon;
    public Transform groupTransformFlip;
    public BoxCollider2D colliderBody;
    public Vector2 moveForce;
    public Vector2 jumpForce;
    public Vector2 throwGrenadeDirection;
    public float throwForceValue;
    public float fallForceValue;
    [HideInInspector]
    public Vector2 lastDiePosition;

    [Header("AIM")]
    public Transform straightAimPoint;
    public Transform crossAimPoint;
    public Transform upAimPoint;
    public Transform downAimPoint;
    public Transform crouchAimPoint;

    [Header("EQUIPMENT")]
    public BaseWeapon currentWeapon;

    private WeaponType currentWeaponType;
    private BaseGun normalGun;
    private BaseGun specialGun;
    [SerializeField]
    private BaseGun dropGun;
    private BaseMeleeWeapon meleeWeapon;
    private BaseGrenade grenadePrefab;
    private int numberOfGrenade;
    private int grenadeLevel;
    private float meleeAttackRate;

    [Header("SPINE")]
    public SkeletonAnimation skeletonAnimation;
    public SkeletonRenderer skeletonRenderer;
    public SkeletonUtilityBone aimBone;
    [SpineAnimation]
    public string crouch, move, jump, lookDown, throwGrenade, victory, parachute, fallBackward;
    [SpineAnimation]
    public string idle, idleRifle, idlePistol, idleInJet;
    [SpineAnimation]
    public string shoot, shootRifle, shootPistol;
    [SpineAnimation]
    public string aim, aimRifle, aimPistol;
    [SpineAnimation]
    public string meleeAttack, knife, pan, guitar;
    [SpineAnimation]
    public List<string> dieAnimationNames;
    [SpineBone]
    public string equipGunBoneName, equipMeleeWeaponBoneName, effectWindBoneName;
    [SpineEvent]
    public string eventFootstep, eventMeleeAttack, eventThrowGrenade;

    private Vector2 idleAimPointPosition;
    private Vector2 crouchAimPointPosition;

    [Header("ACTION CONFIG")]
    public bool enableAttack = true;
    public bool enableMoving = true;
    public bool enableJumping = true;
    public bool enableAiming = true;
    public bool enableCrouching = true;
    public bool enableFlipX = true;

    [Header("STATE")]
    public PlayerState state;
    public bool isGrounded;

    [Header("EFFECT")]
    public ParticleSystem effectDustGround;
    public ParticleSystem effectRestoreHP;
    public GameObject effectStun;
    public GameObject effectImmortal;

    [Space(20f)]
    public BaseSkillTree skillTreePrefab;
    public LayerMask layerMaskCheckObstacle;
    public AudioClip soundGetItemDrop, soundMoveUnderWater, soundRevive, soundChangeWeapon, soundLowHp, soundKnifeKill, soundGrenadeKill;

    protected BaseSkillTree skillTree;

    private float inputHorizontalValue;
    private float inputVerticalValue;
    private float lastTimeShoot;
    private float lastTimeMeleeAttack;
    private float defaultReviveImmortalTime = 3f;
    private bool isUsingHorizontal;
    [HideInInspector]
    public bool isUsingVerticalUp;
    [HideInInspector]
    public bool isUsingVerticalDown;
    private bool isUsingVerticalCross;
    private bool flagJump;
    private bool flagLookDown;
    private bool flagLookUp;
    private bool flagLookCross;
    private bool flagSpawnCrossBullet;
    private bool flagMeleeAttack;
    private bool flagThrowGrenade;
    private bool flagAnimVictory;
    public bool isFiring;
    private bool isAutoFire;
    protected bool isCooldownGrenade;
    private bool isUsingNormalGun = true;
    private bool isBlinkingEffect;
    private bool isDamageBuffed;
    private bool isCriticalBuffed;
    private bool isSpeedBuffed;
    private bool isRenew;
    private int countRevive;
    private int countComboKill;
    private int countComboKillBySpecialGun;
    private List<BaseUnit> nearbyEnemies = new List<BaseUnit>();
    private List<BaseUnit> meleeWeaponVictims = new List<BaseUnit>();
    //private List<BaseModifier> listModifier = new List<BaseModifier>();
    private List<ModifierData> listModifier = new List<ModifierData>();

    public override bool IsFacingRight { get { return !skeletonAnimation.Skeleton.flipX; } }
    public override bool IsMoving { get { return isUsingHorizontal && rigid.velocity.sqrMagnitude != 0f; } }


    public void SetLookDir(bool isFacingRight)
    {
        if (isFacingRight)
        {
            skeletonAnimation.Skeleton.flipX = false;

            Vector3 v = groupTransformFlip.localEulerAngles;
            v.y = 0f;
            groupTransformFlip.localEulerAngles = v;
        }
        else
        {
            skeletonAnimation.Skeleton.flipX = true;

            Vector3 v = groupTransformFlip.localEulerAngles;
            v.y = 180f;
            groupTransformFlip.localEulerAngles = v;
        }
    }


    #region UNITY METHODS

    protected override void Awake()
    {
        level = GameData.playerRambos.GetRamboLevel(ProfileManager.UserProfile.ramboId);
        grenadeLevel = GameData.playerGrenades.GetGrenadeLevel(ProfileManager.UserProfile.grenadeId);
        InitSkills();

        base.Awake();
    }

    protected virtual void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonJump, (sender, param) => TryJump());
        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, (sender, param) => TryAttack((bool)param));
        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonThrowGrenade, (sender, param) => TryThrowGrenade());
        EventDispatcher.Instance.RegisterListener(EventID.ToggleSwitchGun, (sender, param) => TrySwitchGun());
        EventDispatcher.Instance.RegisterListener(EventID.ToggleAutoFire, (sender, param) => OnToggleAutoFire());
        EventDispatcher.Instance.RegisterListener(EventID.OutOfAmmo, (sender, param) => OnSpecialGunOutOfAmmo());
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) => OnKillUnit((UnitDieData)param));
        EventDispatcher.Instance.RegisterListener(EventID.TimeOutComboKill, (sender, param) => OnTimeOutComboKill());
        EventDispatcher.Instance.RegisterListener(EventID.GetItemDrop, (sender, param) => OnGetItemDrop((ItemDropData)param));
        EventDispatcher.Instance.RegisterListener(EventID.GetGunDrop, (sender, param) => GetGunDrop((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, (sender, param) => OnReviveByGem());
        EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, (sender, param) => OnReviveByAds());
        EventDispatcher.Instance.RegisterListener(EventID.FinishStage, (sender, param) => OnFinishStage((float)param));
        EventDispatcher.Instance.RegisterListener(EventID.RamboActiveSkill, (sender, param) => OnActiveSkill());
        EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemHP, (sender, param) => OnUseSupportHP());
        EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemGrenade, (sender, param) => OnUseSupportGrenade((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemBooster, (sender, param) => OnUseSupportBooster((BoosterType)param));
        EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemBomb, (sender, param) => OnUseSupportBomb());
        EventDispatcher.Instance.RegisterListener(EventID.CompleteWave, (sender, param) => OnCompleteSurvivalWave());
        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterHP, (sender, param) =>
        {
            float amount = stats.MaxHp * 0.4f;
            RestoreHP(amount, isFromItemDrop: false);
        });

        InitWeapons();

        skeletonAnimation.AnimationState.Start += HandleAnimationStart;
        skeletonAnimation.AnimationState.Complete += HandleAnimationCompleted;
        skeletonAnimation.AnimationState.Event += HandleAnimationEvent;
    }

    void Update()
    {
        if (isDead || IsDisableAction)
            return;

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, layerMaskCheckObstacle);

        AvoidSlideOnInclinedPlane();
        ProcessInput();
        Attack();
        ActiveAim(!flagMeleeAttack && !flagThrowGrenade);
        UpdateDirection();
        TrackAimPoint();
        //TrackIdleTime();
    }

    void FixedUpdate()
    {
        if (isDead || IsDisableAction)
            return;

        Move();
        Jump();
    }

    void OnDisable()
    {
        rigid.angularDrag = 0;
        rigid.velocity = Vector3.zero;
    }

    void OnDestroy()
    {
        OnOutGamePlay();
    }

    void OnApplicationPause(bool pause)
    {
        isFiring = false;
    }

    #endregion


    #region BASE UNIT IMPLEMENTION

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_RAMBO, id, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void Move()
    {
        if (enableMoving)
        {
            if (state == PlayerState.Move || state == PlayerState.Jump)
            {
                if (isUsingHorizontal)
                {
                    if (inputHorizontalValue * rigid.velocity.x < stats.MoveSpeed)
                    {
                        rigid.AddForce(moveForce * inputHorizontalValue, ForceMode2D.Impulse);
                    }
                }

                if (rigid.velocity.x > stats.MoveSpeed || rigid.velocity.x < -stats.MoveSpeed)
                {
                    Vector2 currentVelocity = rigid.velocity;
                    currentVelocity.x = inputHorizontalValue < 0 ? -stats.MoveSpeed : stats.MoveSpeed;
                    rigid.velocity = currentVelocity;
                }
            }
        }
    }

    protected override void Jump()
    {
        if (flagJump && enableJumping)
        {
            flagJump = false;
            rigid.AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }

    protected override void Die()
    {
        base.Die();

        CameraFollow.Instance.slowMotion.Show();
        CameraFollow.Instance.SetGrayScaleEffect(true);
        GameController.Instance.IsPaused = true;
        GameController.Instance.SetActiveAllUnits(false);
        lastDiePosition = transform.position;
        SwitchState(PlayerState.Die);

        EventDispatcher.Instance.PostEvent(EventID.PlayerDie);
    }

    protected override void Attack()
    {
        if (!enableAttack || flagThrowGrenade || flagMeleeAttack)
            return;

        if (isAutoFire && currentWeaponType == WeaponType.NormalGun)
        {
            isFiring = true;
        }

        if (isFiring /*&& !flagLookUp*/)
        {
            float currentTime = Time.time;

            if (nearbyEnemies.Count > 0)
            {
                if (currentTime - lastTimeMeleeAttack > meleeAttackRate)
                {
                    SwitchWeapon(WeaponType.MeleeWeapon);
                    LookStraight();
                }
            }

            float time = currentWeaponType == WeaponType.MeleeWeapon ? lastTimeMeleeAttack : lastTimeShoot;

            if (currentTime - time > stats.AttackRate)
            {
                PlayAnimationAttack();

                if (currentWeaponType == WeaponType.MeleeWeapon)
                {
                    lastTimeMeleeAttack = currentTime;
                }
                else
                {
                    lastTimeShoot = currentTime;
                    AttackData gunAtkData = GetGunAttackData();
                    currentWeapon.Attack(gunAtkData);

                    if (currentWeaponType == WeaponType.SpecialGun)
                    {
                        UIController.Instance.UpdateGunTypeText(isUsingNormalGun, specialGun.ammo);
                    }
                    else if (currentWeaponType == WeaponType.DropGun)
                    {
                        UIController.Instance.UpdateGunTypeText(isUsingNormalGun, dropGun.ammo);
                    }
                }
            }
        }
    }

    protected override void UpdateDirection()
    {
        if (enableFlipX == false)
            return;

        if (inputHorizontalValue > StaticValue.HORIZONTAL_VALUE_CHANGE)
        {
            if (skeletonAnimation.Skeleton.flipX == true)
            {
                skeletonAnimation.Skeleton.flipX = false;

                Vector3 v = groupTransformFlip.localEulerAngles;
                v.y = 0f;
                groupTransformFlip.localEulerAngles = v;
            }
        }
        else if (inputHorizontalValue < -StaticValue.HORIZONTAL_VALUE_CHANGE)
        {
            if (skeletonAnimation.Skeleton.flipX == false)
            {
                skeletonAnimation.Skeleton.flipX = true;

                Vector3 v = groupTransformFlip.localEulerAngles;
                v.y = 180f;
                groupTransformFlip.localEulerAngles = v;
            }
        }
    }

    protected override void EffectTakeDamage()
    {
        if (!isBlinkingEffect)
        {
            isBlinkingEffect = true;
            DOTween.To(ColorSetter, 1f, 0f, 0.1f).OnComplete(ChangeColorToDefault);
        }

        UIController.Instance.takeDamageScreen.Rewind();
        UIController.Instance.takeDamageScreen.Play();
    }

    protected override void AvoidSlideOnInclinedPlane()
    {
        if (isGrounded && !isUsingHorizontal && !IsDisableAction)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public override AttackData GetCurentAttackData()
    {
        AttackData attackData = new AttackData(this, stats.Damage);
        attackData.weapon = isUsingNormalGun ? WeaponType.NormalGun : WeaponType.SpecialGun;

        return attackData;
    }

    public override void ReloadStats()
    {
        stats.ResetToBaseStats();
        currentWeapon.ApplyOptions(this);
        CalculateBaseStatsIncrease();

        if (isRenew)
        {
            isRenew = false;
            stats.SetStats(stats.MaxHp, StatsType.Hp);
        }

        ApplyModifier();
    }

    protected virtual void CalculateBaseStatsIncrease()
    {
        if (GameData.mode == GameMode.Campaign)
        {
            GameData.isAutoCollectCoin = false;
            isDamageBuffed = false;
            isCriticalBuffed = false;
            isSpeedBuffed = false;

            for (int i = 0; i < GameData.selectingBoosters.Count; i++)
            {
                BoosterType type = GameData.selectingBoosters[i];

                switch (type)
                {
                    case BoosterType.CoinMagnet:
                        GameData.isAutoCollectCoin = true;
                        break;

                    case BoosterType.Critical:
                        isCriticalBuffed = true;
                        stats.AdjustStats(stats.CriticalRate * 0.1f, StatsType.CriticalRate);
                        break;

                    case BoosterType.Damage:
                        isDamageBuffed = true;
                        stats.AdjustStats(stats.Damage * 0.1f, StatsType.Damage);
                        break;

                    case BoosterType.Speed:
                        isSpeedBuffed = true;
                        stats.AdjustStats(baseStats.MoveSpeed * 0.1f, StatsType.MoveSpeed);
                        break;
                }
            }
        }
    }

    public override void AddModifier(ModifierData data)
    {
        listModifier.Add(data);
    }

    public override void RemoveModifier(ModifierData data)
    {
        for (int i = 0, cnt = listModifier.Count; i < cnt; i++)
        {
            if (listModifier[i].stats == data.stats && listModifier[i].type == data.type && listModifier[i].value == data.value)
            {
                listModifier.RemoveAt(i);
                break;
            }
        }
    }

    public override void ApplyModifier()
    {
        for (int i = 0; i < listModifier.Count; i++)
        {
            ModifierData modifier = listModifier[i];

            float value = 0;

            switch (modifier.stats)
            {
                case StatsType.Hp:
                    break;

                case StatsType.MaxHp:
                    value = modifier.type == ModifierType.AddPoint ? modifier.value : baseStats.HP * modifier.value;
                    stats.AdjustStats(value, StatsType.MaxHp);
                    break;

                case StatsType.Damage:
                    value = modifier.type == ModifierType.AddPoint ? modifier.value : stats.Damage * modifier.value;
                    stats.AdjustStats(value, StatsType.Damage);
                    break;

                case StatsType.MoveSpeed:
                    value = modifier.type == ModifierType.AddPoint ? modifier.value : baseStats.MoveSpeed * modifier.value;
                    stats.AdjustStats(value, StatsType.MoveSpeed);
                    break;

                case StatsType.AttackTimePerSecond:
                    value = modifier.type == ModifierType.AddPoint ? modifier.value : stats.AttackTimePerSecond * modifier.value;
                    stats.AdjustStats(value, StatsType.AttackTimePerSecond);
                    break;

                case StatsType.CriticalRate:
                    value = modifier.type == ModifierType.AddPoint ? modifier.value : stats.CriticalRate * modifier.value;
                    stats.AdjustStats(value, StatsType.CriticalRate);
                    break;

                case StatsType.CriticalDamageBonus:
                    value = modifier.type == ModifierType.AddPoint ? modifier.value : stats.CriticalDamageBonus * modifier.value;
                    stats.AdjustStats(value, StatsType.CriticalDamageBonus);
                    break;
            }
        }
    }

    public override void Renew()
    {
        base.Renew();

        isRenew = true;
        listModifier.Clear();

        isImmortal = false;
        flagJump = false;
        flagLookDown = false;
        flagLookUp = false;
        flagSpawnCrossBullet = false;
        flagMeleeAttack = false;
        flagThrowGrenade = false;

        countComboKill = 0;
        countComboKillBySpecialGun = 0;

        UpdateHealthBar();
        ActiveHealthBar(true);
        ActiveSoundLowHp(false);
        skeletonAnimation.ClearState();

        effectStun.SetActive(false);
        effectImmortal.SetActive(false);

        CameraFollow.Instance.SetGrayScaleEffect(false);
    }

    public override void TakeDamage(AttackData attackData)
    {
        if (isDead || attackData.attacker.isDead)
            return;

        if (isImmortal)
        {
            EffectController.Instance.SpawnTextTMP(BodyCenterPoint.position, Color.yellow, "BLOCK", parent: PoolingController.Instance.groupText);
            return;
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
    }

    public override void TakeDamage(float damage)
    {
        if (isDead == false)
        {
            if (isImmortal)
            {
                EffectController.Instance.SpawnTextTMP(BodyCenterPoint.position, Color.yellow, "BLOCK", parent: PoolingController.Instance.groupText);
                return;
            }

            EffectTakeDamage();
            ShowTextDamageTaken(damage);
            stats.AdjustStats(-damage, StatsType.Hp);
            UpdateHealthBar();

            if (stats.HP <= 0)
            {
                Die();
            }
        }
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.UpdatePlayerHpBar(HpPercent);

        if (healthBar != null)
        {
            Vector2 v = healthBar.size;
            v.x = healthBarSizeX * HpPercent;
            healthBar.size = v;
        }
    }

    public override void GetStun(float duration)
    {
        isStun = true;
        effectStun.SetActive(true);
        StopMoving();
        rigid.angularDrag = 0f;
        rigid.velocity = Vector3.zero;

        this.StartDelayAction(() =>
        {
            isStun = false;
            effectStun.SetActive(false);
            rigid.angularDrag = 0.05f;
        }, duration);
    }

    public override void FallBackward(float duration)
    {
        isKnockBack = true;

        Vector2 fallForce;
        fallForce.x = IsFacingRight ? -fallForceValue : fallForceValue;
        fallForce.y = 0;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigid.AddForce(fallForce, ForceMode2D.Impulse);

        PlayAnimationFallbackward();

        if (isUsingNormalGun)
            normalGun.gameObject.SetActive(false);
        else if (dropGun)
            dropGun.gameObject.SetActive(false);
        else if (specialGun)
            specialGun.gameObject.SetActive(false);

        this.StartDelayAction(() =>
        {
            isKnockBack = false;
            PlayAnimationIdle();
            rigid.angularDrag = 0.05f;

            if (isUsingNormalGun)
                normalGun.gameObject.SetActive(true);
            else if (dropGun)
                dropGun.gameObject.SetActive(true);
            else if (specialGun)
                specialGun.gameObject.SetActive(true);

        }, duration);
    }

    #endregion


    #region PRIVATE METHODS

    private void ProcessInput()
    {
        #region INPUT UNITY_STANDALONE
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            EventDispatcher.Instance.PostEvent(EventID.ClickButtonShoot, true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            EventDispatcher.Instance.PostEvent(EventID.ClickButtonShoot, false);
        }
#endif
        #endregion


        if (!enableMoving && !enableAiming)
            return;

        if (IsDisableAction)
        {
            inputHorizontalValue = 0;
            inputVerticalValue = 0;
            isUsingHorizontal = false;
            return;
        }

        inputHorizontalValue = CnInputManager.GetAxis(StaticValue.JOYSTICK_HORIZONTAL);
        isUsingHorizontal = (inputHorizontalValue > StaticValue.HORIZONTAL_VALUE_CHANGE || inputHorizontalValue < -StaticValue.HORIZONTAL_VALUE_CHANGE);

        inputVerticalValue = CnInputManager.GetAxis(StaticValue.JOYSTICK_VERTICAL);
        float f = Mathf.Abs(inputVerticalValue / inputHorizontalValue);
        isUsingVerticalUp = inputVerticalValue > StaticValue.VERTICAL_VALUE_CHANGE && f > 0.5f;
        isUsingVerticalDown = inputVerticalValue < -StaticValue.VERTICAL_VALUE_CHANGE && f > 0.5f;
        //isUsingVerticalCross = inputVerticalValue > StaticValue.VERTICAL_VALUE_CHANGE && f > 0.5f && f < 1f;

        if (isUsingVerticalDown)
        {
            if (isGrounded)
            {
                SwitchState(PlayerState.Crouch);
            }
            else
            {
                LookDown();
            }
        }
        else
        {
            if (currentWeaponType != WeaponType.MeleeWeapon)
            {
                if (isUsingVerticalUp)
                {
                    LookUp();
                }
                else if (isUsingVerticalCross)
                {
                    LookCross();
                }
            }

            if (isGrounded)
            {
                if (isUsingHorizontal)
                {
                    SwitchState(PlayerState.Move);
                }
                else
                {
                    SwitchState(PlayerState.Idle);
                }
            }
        }

        if (isGrounded == false)
        {
            SwitchState(PlayerState.Jump);
        }

        if (isUsingVerticalUp == false && isUsingVerticalCross == false)
        {
            if (isGrounded || (isGrounded == false && isUsingVerticalDown == false))
            {
                LookStraight();
            }
        }
    }

    private void InitSkills()
    {
        skillTree = Instantiate<BaseSkillTree>(skillTreePrefab, transform);
        skillTree.Init(id);
    }

    private void InitWeapons()
    {
        // Normal gun
        BaseGun normalGunPrefab = GameResourcesUtils.GetGunPrefab(ProfileManager.UserProfile.gunNormalId);
        //BaseGun normalGunPrefab = GameResourcesUtils.GetGunPrefab(8);
        if (normalGunPrefab != null)
        {
            BaseGun normalGunInstance = Instantiate(normalGunPrefab, groupWeapon);
            BoneFollower bone = normalGunInstance.gameObject.AddComponent<BoneFollower>();

            if (bone != null)
            {
                bone.skeletonRenderer = skeletonRenderer;
                bone.boneName = equipGunBoneName;
            }

            normalGun = normalGunInstance;
            normalGun.gameObject.name = normalGun.equipmentName;

            int level = GameData.playerGuns.GetGunLevel(ProfileManager.UserProfile.gunNormalId);
            //int level = GameData.playerGuns.GetGunLevel(8);
            normalGun.Init(level);
        }

        // Special gun
        BaseGun specialGunPrefab = GameResourcesUtils.GetGunPrefab(ProfileManager.UserProfile.gunSpecialId);
        //BaseGun specialGunPrefab = GameResourcesUtils.GetGunPrefab(108);
        if (specialGunPrefab != null)
        {
            BaseGun specialGunInstance = Instantiate(specialGunPrefab, groupWeapon);
            BoneFollower bone = specialGunInstance.gameObject.AddComponent<BoneFollower>();

            if (bone != null)
            {
                bone.skeletonRenderer = skeletonRenderer;
                bone.boneName = equipGunBoneName;
            }

            specialGun = specialGunInstance;
            specialGun.gameObject.name = specialGun.equipmentName;
            int level = GameData.playerGuns.GetGunLevel(ProfileManager.UserProfile.gunSpecialId);
            //int level = GameData.playerGuns.GetGunLevel(108);
            specialGun.Init(level);
            specialGun.gameObject.SetActive(false);
        }
        else
        {
            UIController.Instance.buttonSwitchGun.Disable();
        }

        // Melee Weapon
        BaseMeleeWeapon meleeWeaponPrefab = GameResourcesUtils.GetMeleeWeaponPrefab(ProfileManager.UserProfile.meleeWeaponId);
        //BaseMeleeWeapon meleeWeaponPrefab = GameResourcesUtils.GetMeleeWeaponPrefab(601);
        if (meleeWeaponPrefab != null)
        {
            BaseMeleeWeapon meleeWeaponInstance = Instantiate(meleeWeaponPrefab, groupWeapon);
            BoneFollower bone = meleeWeaponInstance.gameObject.AddComponent<BoneFollower>();

            if (bone != null)
            {
                bone.skeletonRenderer = skeletonRenderer;
                bone.boneName = equipMeleeWeaponBoneName;
            }

            meleeWeapon = meleeWeaponInstance;
            meleeWeapon.gameObject.name = meleeWeapon.equipmentName;
            int level = GameData.playerMeleeWeapons.GetMeleeWeaponLevel(ProfileManager.UserProfile.meleeWeaponId);
            //int level = GameData.playerMeleeWeapons.GetMeleeWeaponLevel(601);
            meleeWeapon.Init(level);
            meleeWeapon.InitEffect(skeletonAnimation, effectWindBoneName);
            meleeWeapon.gameObject.SetActive(false);
            meleeAttackRate = 1f / meleeWeapon.baseStats.AttackTimePerSecond;
            DebugCustom.Log("Melee Attack Rate=" + meleeAttackRate);
        }

        // Grenade
        int grenadeId = ProfileManager.UserProfile.grenadeId;
        grenadePrefab = GameResourcesUtils.GetGrenadePrefab(grenadeId);
        numberOfGrenade = GameData.playerGrenades.ContainsKey(grenadeId) ? GameData.playerGrenades[grenadeId].quantity : 0;
        UIController.Instance.UpdateGrenadeText(numberOfGrenade);

        if (numberOfGrenade <= 0)
            UIController.Instance.ActiveButtonGrenade(false);

        //if (specialGun)
        //{
        //    normalGun.gameObject.SetActive(false);
        //    SwitchWeapon(WeaponType.SpecialGun);
        //}
        //else
        //{
        //    SwitchWeapon(WeaponType.NormalGun);
        //}

        SwitchWeapon(WeaponType.NormalGun);

        if (GameData.isAutoFire)
        {
            UIController.Instance.ToggleAutoFire();
        }
    }

    protected virtual void SwitchWeapon(WeaponType weaponType)
    {
        if (currentWeaponType == weaponType)
            return;

        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        switch (weaponType)
        {
            case WeaponType.NormalGun:
                currentWeapon = normalGun;
                isUsingNormalGun = true;
                break;

            case WeaponType.SpecialGun:
                currentWeapon = specialGun;
                isUsingNormalGun = false;
                break;

            case WeaponType.MeleeWeapon:
                currentWeapon = meleeWeapon;
                break;

            case WeaponType.DropGun:
                currentWeapon = dropGun;
                isUsingNormalGun = false;
                break;
        }

        flagMeleeAttack = false;
        flagThrowGrenade = false;
        currentWeapon.gameObject.SetActive(true);
        currentWeaponType = weaponType;

        if (currentWeapon is BaseGun)
        {
            UIController.Instance.UpdateGunTypeText(isUsingNormalGun, ((BaseGun)currentWeapon).ammo);
            idle = ((BaseGun)currentWeapon).gunType == GunType.Pistol ? idlePistol : idleRifle;
            shoot = ((BaseGun)currentWeapon).gunType == GunType.Pistol ? shootPistol : shootRifle;
            aim = ((BaseGun)currentWeapon).gunType == GunType.Pistol ? aimPistol : aimRifle;
        }

        ReloadStats();
    }

    private void SwitchState(PlayerState newState)
    {
        if (state == newState)
            return;

        state = newState;

        switch (state)
        {
            case PlayerState.Idle:
                //lastTimeIdle = Time.time;
                PlayAnimationIdle();
                StopMoving();
                break;

            case PlayerState.Move:
                if (enableMoving)
                {
                    PlayAnimationMove();
                }

                break;

            case PlayerState.Crouch:
                if (enableCrouching)
                {
                    PlayAnimationCrouch();
                    StopMoving();
                }

                break;

            case PlayerState.Jump:
                if (enableJumping)
                {
                    PlayAnimationJump();
                }

                break;

            case PlayerState.Die:
                PlayAnimationDie();
                break;
        }

        ResizeColliderBody(state == PlayerState.Crouch);
    }

    public virtual AttackData GetGunAttackData()
    {
        //float critFromGun = Mathf.Clamp01(((BaseGun)currentWeapon).baseStats.CriticalRate / 100f);
        //float critFinal = isCriticalBuffed ? Mathf.Clamp01(critFromGun + 0.1f) : critFromGun;
        //bool isCritical = Random.Range(0f, 1f) <= critFinal;
        bool isCritical = Random.Range(0f, 1f) <= stats.CriticalRate / 100f;

        float damage = stats.Damage;

        if (isCritical)
        {
            float ratio = 1 + ((BaseGun)currentWeapon).baseStats.CriticalDamageBonus / 100f;
            damage *= ratio;
        }

        AttackData attackData = new AttackData(this, damage, isCritical: isCritical);
        attackData.weapon = isUsingNormalGun ? WeaponType.NormalGun : WeaponType.SpecialGun;
        attackData.weaponId = ((BaseGun)currentWeapon).id;

        return attackData;
    }

    public virtual AttackData GetMeleeWeaponAttackData()
    {
        //float critFromWeapon = Mathf.Clamp01(meleeWeapon.baseStats.CriticalRate / 100f);
        //float critFinal = isCriticalBuffed ? Mathf.Clamp01(critFromWeapon + 0.1f) : critFromWeapon;
        //bool isCritical = Random.Range(0f, 1f) <= critFinal;
        bool isCritical = Random.Range(0f, 1f) <= stats.CriticalRate / 100f;

        float damage = stats.Damage;

        if (isCritical)
        {
            float ratio = 1 + (meleeWeapon.baseStats.CriticalDamageBonus / 100f);
            damage *= ratio;
        }

        AttackData attackData = new AttackData(this, damage, isCritical: isCritical);
        attackData.weapon = WeaponType.MeleeWeapon;

        return attackData;
    }

    public virtual AttackData GetGrenadeAttackData(BaseGrenade grenade)
    {
        float grenadeDamage = isDamageBuffed ? grenade.baseStats.Damage * 1.15f : grenade.baseStats.Damage;
        AttackData atkData = new AttackData(this, grenadeDamage, grenade.baseStats.Radius, weapon: WeaponType.Grenade);

        return atkData;
    }

    protected virtual float GetReviveImmortalDuration()
    {
        return defaultReviveImmortalTime;
    }

    private void Revive(float hpPercent, Vector2 position)
    {
        Renew();
        SwitchState(PlayerState.Idle);
        currentWeaponType = WeaponType.None;
        SwitchWeapon(WeaponType.NormalGun);
        float hp = stats.MaxHp * hpPercent;
        stats.SetStats(hp, StatsType.Hp);
        UpdateHealthBar();
        ActiveSoundLowHp(HpPercent < 0.2f);
        transform.position = position;
        isFiring = false;
        colliderBody.enabled = false;
        colliderBody.enabled = true;

        EffectController.Instance.SpawnTextTMP(BodyCenterPoint.position, Color.green, string.Format("+{0}% HP", hpPercent * 100), parent: PoolingController.Instance.groupText);
        GameController.Instance.IsPaused = false;
        SoundManager.Instance.PlaySfx(soundRevive);
        GameData.isUseRevive = true;

        float immortalDuration = GetReviveImmortalDuration();
        StartCoroutine(CoroutineImmortal(immortalDuration));
    }

    private void LookUp()
    {
        if (flagLookDown)
        {
            flagLookDown = false;
            ActiveLookDown(false);
        }

        flagLookUp = true;
        flagLookCross = false;

        if (flagSpawnCrossBullet == false)
        {
            flagSpawnCrossBullet = true;

            if (isFiring)
            {
                if (currentWeapon is BaseGun && stats.AttackRate <= 0.2f)
                {
                    ((BaseGun)currentWeapon).ReleaseCrossBullets(GetGunAttackData(), spawnCrossBulletPoint, IsFacingRight);
                }
            }
        }
    }

    private void LookCross()
    {
        if (flagLookDown)
        {
            flagLookDown = false;
            ActiveLookDown(false);
        }

        flagLookUp = false;
        flagLookCross = true;
    }

    private void LookDown()
    {
        if (!enableAiming || isOnVehicle)
            return;


        if (flagLookDown == false)
        {
            flagLookDown = true;
            PlayAnimationLookDown();
        }

        flagLookUp = false;
        flagLookCross = false;
        flagSpawnCrossBullet = false;
    }

    private void LookStraight()
    {
        if (flagLookDown)
        {
            flagLookDown = false;
            ActiveLookDown(false);
        }

        flagLookUp = false;
        flagLookCross = false;
        flagSpawnCrossBullet = false;
    }

    private void TrackAimPoint()
    {
        if (flagLookUp)
        {
            aimBone.transform.position = upAimPoint.position;
        }
        else if (flagLookDown)
        {
            aimBone.transform.position = downAimPoint.position;
        }
        else if (flagLookCross)
        {
            aimBone.transform.position = crossAimPoint.position;
        }
        else if (state == PlayerState.Crouch)
        {
            aimBone.transform.position = crouchAimPoint.position;
        }
        else
        {
            aimBone.transform.position = straightAimPoint.position;
        }
    }

    protected virtual void ReleaseGrenade()
    {
        BaseGrenade grenade = ((BaseGrenade)grenadePrefab).Create();
        grenade.Init(grenadeLevel);
        AttackData atkData = GetGrenadeAttackData(grenade);
        Vector2 v = throwGrenadeDirection * throwForceValue;
        v.x = IsFacingRight ? v.x : -v.x;

        grenade.Active(atkData, throwGrenadePoint.position, v, PoolingController.Instance.groupGrenade);

        numberOfGrenade--;

        if (numberOfGrenade <= 0)
            UIController.Instance.ActiveButtonGrenade(false);
        else
            StartCoroutine(CoroutineCooldownGrenade(grenade.baseStats.Cooldown));

        UIController.Instance.UpdateGrenadeText(numberOfGrenade);
        GameData.playerGrenades.Consume(ProfileManager.UserProfile.grenadeId, 1);
    }

    private void DealMeleeWeaponDamage()
    {
        for (int i = 0; i < meleeWeaponVictims.Count; i++)
        {
            BaseUnit unit = meleeWeaponVictims[i];

            if (unit.CompareTag(StaticValue.TAG_ENEMY))
            {
                AttackData meleeWeaponAtkData = GetMeleeWeaponAttackData();
                unit.TakeDamage(meleeWeaponAtkData);
            }
        }
    }

    protected virtual void RestoreHP(float value, bool isFromItemDrop)
    {
        float hpRestore = value;

        if (isFromItemDrop)
        {
            hpRestore = (stats.MaxHp - stats.HP) * 0.5f;

            if (hpRestore < 500)
                hpRestore = 500;
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

    private void RestoreFullHp()
    {
        float hpToFull = stats.MaxHp - stats.HP;
        RestoreHP(hpToFull, isFromItemDrop: false);
    }

    private IEnumerator CoroutineImmortal(float duration)
    {
        float timer = 0;
        WaitForSeconds colorChangeInterval = new WaitForSeconds(0.2f);
        isImmortal = true;
        effectImmortal.SetActive(true);

        while (timer < duration)
        {
            timer += 0.2f;
            Color c = skeletonAnimation.skeleton.GetColor();
            c.a = c.a == 1f ? 0.5f : 1f;
            skeletonAnimation.skeleton.SetColor(c);
            yield return colorChangeInterval;
        }

        isImmortal = false;
        effectImmortal.SetActive(false);
        ChangeColorToDefault();
    }

    private void ColorSetter(float pNewValue)
    {
        skeletonAnimation.skeleton.g = pNewValue;
        skeletonAnimation.skeleton.b = pNewValue;
    }

    private void ChangeColorToDefault()
    {
        skeletonAnimation.skeleton.SetColor(Color.white);
        isBlinkingEffect = false;
    }

    private void TrackIdleTime()
    {
        if (GameData.mode != GameMode.Campaign)
            return;

        if (state == PlayerState.Idle && isFiring == false)
        {
            //float currentTime = Time.time;

            //if (currentTime - lastTimeIdle > GameController.Instance.CampaignMap.timeAutoSpawn)
            //{
            //    lastTimeIdle = Time.time;
            //    ((CampaignModeController)GameController.Instance.modeController).AutoSpawnEnemy();
            //}
        }
        else
        {

        }
    }

    private void ResizeColliderBody(bool isCrouch)
    {
        if (isCrouch)
        {
            Vector2 v = colliderBody.offset;
            v.y = 0.4f;
            colliderBody.offset = v;

            v = colliderBody.size;
            v.y = 0.6f;
            colliderBody.size = v;
        }
        else
        {
            Vector2 v = colliderBody.offset;
            v.y = 0.63f;
            colliderBody.offset = v;

            v = colliderBody.size;
            v.y = 1.09f;
            colliderBody.size = v;
        }
    }

    protected virtual IEnumerator CoroutineCooldownGrenade(float cooldown)
    {
        isCooldownGrenade = true;
        UIController.Instance.SetCooldownButtonGrenade(false);
        float count = 0;

        while (isCooldownGrenade)
        {
            count += Time.deltaTime;
            isCooldownGrenade = count < cooldown;
            float percentCooldown = Mathf.Clamp01(count / cooldown);
            UIController.Instance.imageCooldownGrenade.fillAmount = percentCooldown;
            UIController.Instance.textCooldownGrenade.text = string.Format("{0:f1}", cooldown - count);
            yield return null;
        }

        isCooldownGrenade = false;
        UIController.Instance.SetCooldownButtonGrenade(true);
    }

    protected void ActiveSoundLowHp(bool isActive)
    {
        if (isActive)
        {
            if (audioSource.clip == null)
            {
                audioSource.loop = true;
                audioSource.clip = soundLowHp;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        UIController.Instance.alarmRedScreen.SetActive(isActive);
    }

    private void GetGunDrop(int id)
    {
        if (GameData.staticGunData.ContainsKey(id))
        {
            if (dropGun == null || dropGun.id != id)
            {
                BaseGun gunPrefab = GameResourcesUtils.GetGunPrefab(id);
                BaseGun dropGunInstance = Instantiate(gunPrefab, groupWeapon);
                BoneFollower bone = dropGunInstance.gameObject.AddComponent<BoneFollower>();

                if (bone != null)
                {
                    bone.skeletonRenderer = skeletonRenderer;
                    bone.boneName = equipGunBoneName;
                }


                if (dropGun)
                {
                    Destroy(dropGun.gameObject);
                    dropGun = null;
                    currentWeaponType = WeaponType.None;
                }

                dropGun = dropGunInstance;
                dropGun.gameObject.name = dropGun.equipmentName;
                dropGun.Init(1);

                SO_GunStats staticGun = GameData.staticGunData.GetBaseStats(id, 1);
                dropGun.ammo = staticGun.Ammo;
                dropGun.gameObject.SetActive(false);

                SwitchWeapon(WeaponType.DropGun);

                //if (specialGun != null && specialGun.id != id)
                //    UIController.Instance.hudGunDrop.Open(id);
            }
            else
            {
                dropGun.ammo = dropGun.baseStats.Ammo;
            }

            UIController.Instance.buttonSwitchGun.Enable();
        }
        else
        {
            DebugCustom.Log("Invalid gun id=" + id);
        }
    }

    private void UnequipGunDrop()
    {
        if (dropGun)
        {
            dropGun.gameObject.SetActive(false);
            dropGun = null;

            if (specialGun == null)
            {
                UIController.Instance.buttonSwitchGun.Disable();
            }
        }

        SwitchWeapon(WeaponType.NormalGun);
    }

    public void OnEnemyEnterNearby(BaseUnit unit)
    {
        if (nearbyEnemies.Contains(unit) == false)
        {
            nearbyEnemies.Add(unit);
        }
    }

    public void OnEnemyExitNearby(BaseUnit unit)
    {
        if (nearbyEnemies.Contains(unit))
        {
            nearbyEnemies.Remove(unit);
        }
    }

    #endregion


    #region LISTENERS

    private void TryJump()
    {
        if (isGrounded && isOnVehicle == false)
        {
            //transform.parent = null;

            Vector2 v = rigid.velocity;
            v.y = 0;
            rigid.velocity = v;

            flagJump = true;
        }
    }

    private void TryAttack(bool isAttack)
    {
        //if (isAutoFire == false)
        isFiring = isAttack;

        // If stop shooting when idle, play animation idle
        if (isAttack == false)
        {
            if (state == PlayerState.Idle)
            {
                PlayAnimationIdle();
            }
        }
    }

    private void TryThrowGrenade()
    {
        if (isCooldownGrenade == false && numberOfGrenade > 0 && flagThrowGrenade == false)
        {
            flagThrowGrenade = true;
            flagMeleeAttack = false;
            PlayAnimationThrow();
            EventDispatcher.Instance.PostEvent(EventID.UseGrenade);
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_THROW_GRENADE);
        }
    }

    private void TrySwitchGun()
    {
        if (isUsingNormalGun)
        {
            if (dropGun)
            {
                SwitchWeapon(WeaponType.DropGun);
            }
            else if (specialGun)
            {
                SwitchWeapon(WeaponType.SpecialGun);
            }

            isFiring = false;
        }
        else
        {
            SwitchWeapon(WeaponType.NormalGun);
        }

        SoundManager.Instance.PlaySfx(soundChangeWeapon);
    }

    private void OnToggleAutoFire()
    {
        isAutoFire = !isAutoFire;

        if (isAutoFire == false)
        {
            isFiring = false;
        }

        GameData.isAutoFire = isAutoFire;
    }

    private void OnSpecialGunOutOfAmmo()
    {
        if (currentWeaponType == WeaponType.SpecialGun)
        {
            SwitchWeapon(WeaponType.NormalGun);
        }
        else if (currentWeaponType == WeaponType.DropGun)
        {
            UnequipGunDrop();
        }
        else
        {
            DebugCustom.Log("Current weapon is NORMAL GUN");
        }
    }

    private void OnKillUnit(UnitDieData data)
    {
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (enemy.isMiniBoss || enemy.isFinalBoss || data.attackData == null)
            return;

        countComboKill++;
        EventDispatcher.Instance.PostEvent(EventID.GetComboKill, countComboKill);

        // Kill by special weapon
        if (data.attackData.weapon == WeaponType.SpecialGun)
        {
            countComboKillBySpecialGun++;
            EventDispatcher.Instance.PostEvent(EventID.GetComboKillBySpecialGun, countComboKillBySpecialGun);
            EventDispatcher.Instance.PostEvent(EventID.KillEnemyBySpecialGun);
        }
        else
        {
            countComboKillBySpecialGun = 0;
        }

        // Kill by knife
        if (data.attackData.weapon == WeaponType.MeleeWeapon)
        {
            if (data.attackData.weaponId == StaticValue.MELEE_WEAPON_ID_KNIFE)
                PlaySound(soundKnifeKill);

            EventDispatcher.Instance.PostEvent(EventID.KillEnemyByKnife);

            Vector2 v = enemy.transform.position;
            v.y += 2f;
            ((BaseMeleeWeapon)meleeWeapon).SpawnEffectText(v);
        }

        // Kill by grenade
        if (data.attackData.weapon == WeaponType.Grenade)
        {
            PlaySound(soundGrenadeKill);
            EventDispatcher.Instance.PostEvent(EventID.KillEnemyByGrenade);
        }
    }

    private void OnTimeOutComboKill()
    {
        countComboKill = 0;
        countComboKillBySpecialGun = 0;
    }

    private void OnGetItemDrop(ItemDropData data)
    {
        switch (data.type)
        {
            case ItemDropType.Health:
                RestoreHP(data.value, isFromItemDrop: true);
                break;

            case ItemDropType.Coin:
                EffectController.Instance.SpawnTextTMP(BodyCenterPoint.position, Color.yellow, string.Format("+{0}", data.value), parent: PoolingController.Instance.groupText);
                break;

            case ItemDropType.GunSpread:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_SPREAD);
                break;

            case ItemDropType.GunRocketChaser:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_ROCKET_CHASER);
                break;

            case ItemDropType.GunFamas:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_FAMAS);
                break;

            case ItemDropType.GunLaser:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_LASER);
                break;

            case ItemDropType.GunSplit:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_SPLIT);
                break;

            case ItemDropType.GunFireBall:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_FIRE_BALL);
                break;

            case ItemDropType.GunTesla:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_TESLA);
                break;

            case ItemDropType.GunKamePower:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_KAME_POWER);
                break;

            case ItemDropType.GunFlame:
                EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, StaticValue.GUN_ID_FLAME);
                break;
        }

        SoundManager.Instance.PlaySfx(soundGetItemDrop);
    }

    private void OnReviveByGem()
    {
        countRevive++;
        Revive(0.5f, lastDiePosition);
        CameraFollow.Instance.ResetCameraToPlayer();
    }

    private void OnReviveByAds()
    {
        countRevive++;
        Revive(0.3f, lastDiePosition);
        CameraFollow.Instance.ResetCameraToPlayer();
    }

    protected virtual void OnFinishStage(float delayEndGame)
    {
        isImmortal = true;

        this.StartDelayAction(() =>
        {
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_VOICE_VICTORY);
            PlayAnimationVictory();

        }, delayEndGame);
    }

    private void OnOutGamePlay()
    {
        if (specialGun != null)
        {
            GameData.playerGuns.SetGunAmmo(ProfileManager.UserProfile.gunSpecialId, specialGun.ammo);
        }
    }

    protected virtual void OnCompleteSurvivalWave()
    {
        if (isDamageBuffed)
        {
            isDamageBuffed = false;
            RemoveModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, 0.15f));
        }

        if (isCriticalBuffed)
        {
            isCriticalBuffed = false;
            RemoveModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, 0.1f));
        }

        if (isSpeedBuffed)
        {
            isSpeedBuffed = false;
            RemoveModifier(new ModifierData(StatsType.MoveSpeed, ModifierType.AddPercentBase, 0.2f));
        }

        ReloadStats();

        GameData.survivalUsingBooster = BoosterType.None;
        UIController.Instance.ActiveBoosters();
    }

    protected virtual void OnActiveSkill()
    {
        if (skillTree.activeSkill)
        {
            skillTree.activeSkill.Excute();
        }
        else
        {
            DebugCustom.LogError("Active skill NULL");
        }
    }

    protected virtual void OnUseSupportHP()
    {
        DebugCustom.Log("Use support item HP");
        RestoreFullHp();
    }

    protected virtual void OnUseSupportGrenade(int quantity)
    {
        DebugCustom.Log("Use support item grenades");

        if (numberOfGrenade <= 0)
        {
            if (quantity > 0)
            {
                UIController.Instance.ActiveButtonGrenade(true);
            }
        }

        numberOfGrenade += quantity;
        UIController.Instance.UpdateGrenadeText(numberOfGrenade);
    }

    protected virtual void OnUseSupportBomb()
    {
        DebugCustom.Log("Use support item bomb");
    }

    protected virtual void OnUseSupportBooster(BoosterType type)
    {
        DebugCustom.Log("Use support item booster " + type);
        GameData.survivalUsingBooster = type;
        UIController.Instance.ActiveBoosters();

        isDamageBuffed = false;
        isCriticalBuffed = false;
        isSpeedBuffed = false;

        if (GameData.mode == GameMode.Survival)
        {
            if (GameData.survivalUsingBooster == BoosterType.Damage)
            {
                isDamageBuffed = true;
                AddModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, 0.15f));
            }
            else if (GameData.survivalUsingBooster == BoosterType.Critical)
            {
                isCriticalBuffed = true;
                AddModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, 0.1f));
            }
            else if (GameData.survivalUsingBooster == BoosterType.Speed)
            {
                isSpeedBuffed = true;
                AddModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, 0.2f));
            }
        }

        ReloadStats();
    }

    #endregion


    #region SPINE & ANIMATION

    protected override void HandleAnimationStart(TrackEntry entry)
    {
        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            meleeWeaponVictims.Clear();
            for (int i = 0; i < nearbyEnemies.Count; i++)
            {
                meleeWeaponVictims.Add(nearbyEnemies[i]);
            }

            currentWeapon.PlaySoundAttack();
            meleeWeapon.ActiveEffect(true);
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventMeleeAttack) == 0)
        {
            DealMeleeWeaponDamage();
        }

        if (string.Compare(e.Data.Name, eventThrowGrenade) == 0)
        {
            ReleaseGrenade();
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        // Knife
        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            flagMeleeAttack = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            meleeWeapon.ActiveEffect(false);

            if (isUsingNormalGun)
            {
                SwitchWeapon(WeaponType.NormalGun);
            }
            else if (dropGun)
            {
                SwitchWeapon(WeaponType.DropGun);
            }
            else
            {
                SwitchWeapon(WeaponType.SpecialGun);
            }
        }

        // Throw
        if (string.Compare(entry.animation.name, throwGrenade) == 0)
        {
            flagThrowGrenade = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }

        // Victory
        if (string.Compare(entry.animation.name, victory) == 0)
        {
            if (flagAnimVictory == false)
            {
                flagAnimVictory = true;

                if (countRevive <= 0)
                {
                    EventDispatcher.Instance.PostEvent(EventID.CompleteStageWithoutReviving);
                }

                UIController.Instance.alarmRedScreen.SetActive(false);

                this.StartDelayAction(() =>
                {
                    EventDispatcher.Instance.PostEvent(EventID.GameEnd, true);
                }, 0.5f);
            }
        }

        // Fallback
        if (string.Compare(entry.animation.name, fallBackward) == 0)
        {
            StopMoving();
            Rigid.angularDrag = 0f;
        }

        // Die
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            if (GameData.mode == GameMode.Campaign)
            {
                if (countRevive > 0)
                {
                    EventDispatcher.Instance.PostEvent(EventID.GameEnd, false);
                }
                else
                {
                    float deltaX = transform.position.x - GameController.Instance.CampaignMap.playerSpawnPoint.position.x;
                    float mapLength = GameController.Instance.CampaignMap.mapEndPoint.position.x - GameController.Instance.CampaignMap.playerSpawnPoint.position.x;
                    float progress = Mathf.Clamp01(deltaX / mapLength);

                    UIController.Instance.hudSaveMe.Open(progress);
                }
            }
            else if (GameData.mode == GameMode.Survival)
            {
                EventDispatcher.Instance.PostEvent(EventID.GameEnd, false);
            }
        }
    }

    private void ActiveAim(bool isActive)
    {
        if (isActive)
        {
            if (skeletonAnimation.AnimationState.GetCurrent(2) != null
                && string.Compare(skeletonAnimation.AnimationState.GetCurrent(2).animation.name, aim) == 0)
                return;

            skeletonAnimation.AnimationState.SetAnimation(2, aim, false);
        }
        else
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
        }
    }

    private void ActiveLookDown(bool isActive)
    {
        if (isActive)
        {
            skeletonAnimation.AnimationState.SetAnimation(3, lookDown, false);
        }
        else
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(3, 0f);
        }
    }

    public void PlayAnimationIdle()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
    }

    public void PlayAnimationIdleInJet()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, idleInJet, true);
    }

    public void PlayAnimationParachute()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, parachute, true);
    }

    private void PlayAnimationCrouch()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, crouch, false);
    }

    private void PlayAnimationMove()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, move, true);
    }

    private void PlayAnimationJump()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, jump, true);
    }

    private void PlayAnimationAttack()
    {
        if (currentWeaponType == WeaponType.MeleeWeapon)
        {
            if (flagMeleeAttack == false)
            {
                flagMeleeAttack = true;

                switch (((BaseMeleeWeapon)currentWeapon).type)
                {
                    case MeleeWeaponType.Knife:
                        meleeAttack = knife;
                        break;
                    case MeleeWeaponType.Pan:
                        meleeAttack = pan;
                        break;
                    case MeleeWeaponType.Guitar:
                        meleeAttack = guitar;
                        break;
                }

                skeletonAnimation.AnimationState.SetAnimation(1, meleeAttack, false);
                //((BaseMeleeWeapon)currentWeapon).ActiveEffect();
            }
        }
        else
        {
            if (state == PlayerState.Idle)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, shoot, false);
                skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0.1f);
            }
            else if (flagThrowGrenade == false)
            {
                skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
                skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
            }
        }
    }

    private void PlayAnimationLookDown()
    {
        ActiveLookDown(true);
    }

    private void PlayAnimationThrow()
    {
        skeletonAnimation.AnimationState.SetAnimation(1, throwGrenade, false).TimeScale = 1.5f;
    }

    private void PlayAnimationDie()
    {
        int index = Random.Range(0, dieAnimationNames.Count);
        string animDie = dieAnimationNames[index];

        skeletonAnimation.AnimationState.SetAnimation(0, animDie, false);
    }

    private void PlayAnimationFallbackward()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, fallBackward, false);
    }

    protected void PlayAnimationVictory()
    {
        skeletonAnimation.ClearState();
        skeletonAnimation.AnimationState.SetAnimation(0, victory, true);
        enabled = false;
    }

    #endregion

}