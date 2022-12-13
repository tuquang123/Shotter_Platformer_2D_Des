using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using DG.Tweening;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class BaseEnemy : BaseUnit
{
    [Header("BASE ENEMY PROPERTIES")]
    public bool canMove = true;
    public bool canJump;
    public bool isMainUnit;
    public bool isRunPassArea = true;
    [HideInInspector]
    public bool isMiniBoss;
    [HideInInspector]
    public bool isFinalBoss;
    [HideInInspector]
    public bool isEffectMeleeWeapon = true;
    [HideInInspector]
    public bool isInvisibleWhenActive;
    public int bounty;
    public EnemyState state;
    public Vector2 moveForce;
    public Vector2 jumpForce;
    public Transform groupTransformPoints;
    public Transform frontCheckPoint;
    public Transform groundCheckPoint;
    [HideInInspector]
    public Vector2 basePosition;
    public FarSensor farSensor;
    public NearSensor nearSensor;
    public Collider2D bodyCollider;
    public Collider2D footCollider;
    public AudioClip soundAttack;
    public LayerMask layerMaskCheckJump;
    public LayerMask layerMaskCheckObstacle;
    [HideInInspector]
    public int zoneId;
    [HideInInspector]
    public int packId;


    [Header("SPINE")]
    public SkeletonAnimation skeletonAnimation;
    public SkeletonUtilityBone aimBone;
    [SpineBone]
    public string gunBone, knifeBone, firePointBone;
    [SpineAnimation]
    public string idle, move, moveFast, shoot, throwGrenade, meleeAttack, aim;
    [SpineAnimation(startsWith = "die")]
    public List<string> dieAnimationNames;
    [SpineEvent]
    public string eventMeleeAttack, eventThrowGrenade, eventShoot;
    [SpineSkin]
    public string defaultSkin, skinMap1, skinMap2, skinMap3;

    [Header("EFFECT")]
    public GameObject effectStun;

    [Space(20f)]
    public BaseUnit target;

    protected bool isNotAllowMoveForward;
    protected bool isRunning;
    [SerializeField]
    protected bool isReadyAttack;
    protected bool isAllowAttackTarget;
    [SerializeField]
    protected bool flagGetCloseToTarget;
    [SerializeField]
    protected bool flagMove = true;
    protected bool flagJumpPassObstacle;
    protected float timeChangePatrolPoint = 3f;
    protected float closeUpRange = 2f;
    protected float lastTimeAttack;
    protected float timeCountPatrol;
    protected Vector3 destinationMove;
    protected List<BaseUnit> nearbyVictims = new List<BaseUnit>();

    private bool isBlinkingEffect;
    private float minTimeMove = 1f;
    private float maxTimeMove = 2f;
    private IEnumerator coroutineHideHealthBar;

    public override bool IsFacingRight { get { return !skeletonAnimation.Skeleton.flipX; } }


    #region UNITY METHODS

    protected override void Awake()
    {
        base.Awake();

        InitSkin();
        InitWeapon();
        InitSortingLayerSpine();
    }

    protected virtual void Start()
    {
        skeletonAnimation.AnimationState.Start += HandleAnimationStart;
        skeletonAnimation.AnimationState.Complete += HandleAnimationCompleted;
        skeletonAnimation.AnimationState.Event += HandleAnimationEvent;
    }

    protected virtual void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            Idle();
            Patrol();
            Attack();
        }
    }

    #endregion


    #region STATE

    protected void SwitchState(EnemyState newState)
    {
        if (state == newState)
            return;

        state = newState;

        switch (state)
        {
            case EnemyState.Idle:
                StartIdle();
                break;

            case EnemyState.Patrol:
                StartPatrol();
                break;

            case EnemyState.Attack:
                StartAttack();
                break;

            case EnemyState.Die:
                StartDie();
                break;
        }
    }

    protected virtual void StartIdle()
    {
        PlayAnimationIdle();
        StopMoving();
        isRunning = false;
    }

    protected virtual void StartPatrol()
    {
        GetRandomPatrolPoint();

        if (isRunning)
        {
            PlayAnimationMoveFast();
        }
        else
        {
            PlayAnimationMove();
        }
    }

    protected virtual void StartAttack()
    {
        timeCountPatrol = 0f;
        StopMoving();
    }

    protected virtual void StartDie()
    {
        target = null;
        ResetAim();
        SetColliderLayers(false);
        ActiveSensor(false);
        PlayAnimationDie();

        List<ItemDropData> items = GetItemDrop();
        GameController.Instance.itemDropController.Spawn(items, BodyCenterPoint.position, this);

        if (isMiniBoss || isFinalBoss)
        {
            if (GameData.mode == GameMode.Campaign)
                UIController.Instance.ActiveIngameUI(false);

            EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionMultiple, transform.position);
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);

            CameraFollow.Instance.slowMotion.Show(callback: () =>
            {
                if (GameData.mode == GameMode.Campaign)
                {
                    EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
                }
                else if (GameData.mode == GameMode.Survival)
                {
                    EventDispatcher.Instance.PostEvent(EventID.BossSurvivalDie);
                }
            });

            if (isFinalBoss)
                EventDispatcher.Instance.PostEvent(EventID.FinalBossDie, id);
        }
    }

    #endregion


    #region BASE UNIT IMPLEMENTION

    protected override void Idle()
    {
        if (state == EnemyState.Idle)
        {
            if (canMove)
            {
                timeCountPatrol += Time.deltaTime;

                if (timeCountPatrol >= timeChangePatrolPoint)
                {
                    timeCountPatrol = 0f;
                    SwitchState(EnemyState.Patrol);
                }
            }
        }
    }

    protected override void Attack() { }

    protected override void Die()
    {
        base.Die();

        StopAllCoroutines();
        skeletonAnimation.ClearState();
        SwitchState(EnemyState.Die);

        //this.StartDelayAction(() =>
        //{
        //    base.Die();
        //    StopAllCoroutines();
        //    SwitchState(EnemyState.Die);
        //}, 0.02f);
    }

    protected override void Move()
    {
        float speed = isRunning ? stats.MoveSpeed * 1.5f : stats.MoveSpeed;

        if (rigid.velocity.x < speed || rigid.velocity.x > -speed)
        {
            Vector2 force = IsFacingRight ? moveForce : -moveForce;
            rigid.AddForce(force, ForceMode2D.Impulse);
        }

        if (rigid.velocity.x > speed || rigid.velocity.x < -speed)
        {
            Vector2 currentVelocity = rigid.velocity;
            currentVelocity.x = IsFacingRight ? stats.MoveSpeed : -stats.MoveSpeed;
            rigid.velocity = currentVelocity;
        }
    }

    protected override void UpdateDirection()
    {
        if (target != null)
        {
            skeletonAnimation.Skeleton.flipX = (target.transform.position.x < transform.position.x);
        }
        else if (transform.position != destinationMove)
        {
            if (canMove)
                skeletonAnimation.Skeleton.flipX = (destinationMove.x < transform.position.x);
        }

        UpdateTransformPoints();
    }

    protected override void EffectTakeDamage()
    {
        if (!isBlinkingEffect)
        {
            isBlinkingEffect = true;
            DOTween.To(ColorSetter, 1f, 0f, 0.1f).OnComplete(ChangeColorToDefault);
        }
    }

    protected override void ApplyDebuffs(AttackData attackData)
    {
        if (attackData.debuffs == null || attackData.debuffs.Count <= 0)
            return;

        base.ApplyDebuffs(attackData);

        for (int i = 0; i < attackData.debuffs.Count; i++)
        {
            DebuffData debuff = attackData.debuffs[i];

            switch (debuff.type)
            {
                case DebuffType.Stun:
                    GetStun(debuff.duration);
                    break;

                case DebuffType.TakeMoreDamageWhenHighHP:
                    if (HpPercent >= 0.5f)
                        attackData.damage *= (1 + (debuff.damage / 100f));
                    break;

                case DebuffType.Reflect:
                    if (isFinalBoss)
                    {
                        attackData.damage = 0;
                    }
                    else
                    {
                        EffectController.Instance.SpawnTextTMP(BodyCenterPoint.position, Color.cyan, "REFLECT", parent: PoolingController.Instance.groupText);
                    }
                    break;
            }
        }
    }

    public override void SetTarget(BaseUnit unit)
    {
        if (target == null)
        {
            target = unit;
            SwitchState(EnemyState.Attack);
        }
    }

    public override void TakeDamage(AttackData attackData)
    {
        if (isImmortal || isDead || attackData.attacker.isDead)
            return;

        if (attackData.isCritical)
        {
            Vector2 v = BodyCenterPoint.position;
            v.y += 1f;
            EffectController.Instance.SpawnTextCRIT(v);

            EventDispatcher.Instance.PostEvent(EventID.GetCriticalHit);
        }

        ApplyDebuffs(attackData);

        if (attackData.damage > 0)
        {
            EffectTakeDamage();
            ShowTextDamageTaken(attackData);
            stats.AdjustStats(-attackData.damage, StatsType.Hp);
        }

        UpdateHealthBar(isAutoHide: true);

        if (stats.HP <= 0)
        {
            Die();
            EventDispatcher.Instance.PostEvent(EventID.UnitDie, new UnitDieData(this, attackData));
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isImmortal || isDead)
            return;

        if (damage > 0)
        {
            EffectTakeDamage();
            ShowTextDamageTaken(damage);
            stats.AdjustStats(-damage, StatsType.Hp);
        }

        UpdateHealthBar(isAutoHide: true);

        if (stats.HP <= 0)
        {
            Die();

            UnitDieData dieData = new UnitDieData(this);
            EventDispatcher.Instance.PostEvent(EventID.UnitDie, dieData);
        }
    }

    public override void Active(int id, int level, Vector2 position)
    {
        base.Active(id, level, position);

        if (isInvisibleWhenActive)
        {
            FadeIn();
        }
    }

    public virtual void Active(EnemySpawnData spawnData)
    {
        if (GameData.mode == GameMode.Campaign)
        {
            int level = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);

            Active(spawnData.id, level, spawnData.position);

            zoneId = spawnData.zoneId;
            packId = spawnData.packId;
            isMainUnit = spawnData.isMainUnit;
            isRunPassArea = spawnData.isRunPassArea;
            canMove = spawnData.isCanMove;
            canJump = spawnData.isCanJump;
            bounty = spawnData.bounty;

            itemDropList = new List<ItemDropData>();

            for (int i = 0; i < spawnData.items.Count; i++)
            {
                ItemDropData data = spawnData.items[i];
                ItemDropData item = new ItemDropData(data.type, data.value, data.dropRate);
                itemDropList.Add(item);
            }
        }
        else
        {
            DebugCustom.LogError("Current mode is not CAMPAIGN");
        }
    }

    public override void Renew()
    {
        base.Renew();

        zoneId = 0;
        packId = 0;
        isMainUnit = false;
        canMove = true;
        canJump = false;
        isRunPassArea = false;
        isImmortal = false;
        flagGetCloseToTarget = false;
        flagJumpPassObstacle = false;
        flagMove = true;
        isEffectMeleeWeapon = true;

        if (effectStun)
            effectStun.SetActive(false);

        bounty = 0;
        target = null;
        transform.parent = null;
        timeChangePatrolPoint = Random.Range(minTimeMove, maxTimeMove);
        SetCloseRange();
        InitPatrolPoint();
        skeletonAnimation.initialFlipX = true;
        UpdateTransformPoints();
        SetColliderLayers(true);
        ActiveSensor(true);
        UpdateHealthBar();
        ActiveHealthBar(false);
        skeletonAnimation.ClearState();
        nearbyVictims.Clear();
        itemDropList.Clear();
        SwitchState(EnemyState.Idle);
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        if (healthBar != null)
        {
            Vector2 v = healthBar.size;
            v.x = healthBarSizeX * HpPercent;
            healthBar.size = v;

            ActiveHealthBar(HpPercent > 0f);

            if (isAutoHide)
            {
                if (coroutineHideHealthBar != null)
                {
                    StopCoroutine(coroutineHideHealthBar);
                    coroutineHideHealthBar = null;
                }

                coroutineHideHealthBar = CoroutineHideHealthBar();
                StartCoroutine(coroutineHideHealthBar);
            }
        }
    }

    public override void Deactive()
    {
        base.Deactive();

        if (coroutineHideHealthBar != null)
        {
            StopCoroutine(coroutineHideHealthBar);
            coroutineHideHealthBar = null;
        }

        GameController.Instance.RemoveUnit(gameObject);
        isInvisibleWhenActive = false;
        gameObject.SetActive(false);
    }

    public override void GetStun(float duration)
    {
        if (isFinalBoss || isMiniBoss || isStun || !isEffectMeleeWeapon)
            return;

        isStun = true;

        if (effectStun)
            effectStun.SetActive(true);

        StopMoving();
        skeletonAnimation.AnimationState.SetAnimation(0, idle, false);
        rigid.angularDrag = 0f;
        rigid.velocity = Vector3.zero;
        enabled = false;

        this.StartDelayAction(() =>
        {
            isStun = false;
            rigid.angularDrag = 0.05f;
            enabled = true;

            if (effectStun)
                effectStun.SetActive(false);

        }, duration);
    }

    public override bool IsOutOfScreen()
    {
        bool isOutOfScreenX = transform.position.x < CameraFollow.Instance.left.position.x - 0.5f || transform.position.x > CameraFollow.Instance.right.position.x + 0.5f;

        return isOutOfScreenX;
    }

    #endregion


    #region VIRTUAL METHODS

    protected virtual void InitSkin()
    {
        string skin = defaultSkin;

        if (GameData.mode == GameMode.Campaign)
        {
            int mapId = int.Parse(GameController.Instance.CampaignMap.stageNameId.Split('.').First());

            switch ((MapType)mapId)
            {
                case MapType.Map_1_Desert:
                    if (!string.IsNullOrEmpty(skinMap1))
                        skin = skinMap1;
                    break;

                case MapType.Map_2_Lab:
                    if (!string.IsNullOrEmpty(skinMap2))
                        skin = skinMap2;
                    break;

                case MapType.Map_3_Jungle:
                    if (!string.IsNullOrEmpty(skinMap3))
                        skin = skinMap3;
                    break;
            }
        }
        else if (GameData.mode == GameMode.Survival)
        {
            int random = Random.Range(0, 3);

            if (random == 0)
            {
                if (!string.IsNullOrEmpty(skinMap1))
                    skin = skinMap1;
            }
            else if (random == 1)
            {
                if (!string.IsNullOrEmpty(skinMap2))
                    skin = skinMap2;
            }
            else if (random == 2)
            {
                if (!string.IsNullOrEmpty(skinMap3))
                    skin = skinMap3;
            }
        }

        skeletonAnimation.Skeleton.SetSkin(skin);
    }

    protected virtual void InitWeapon() { }

    protected virtual void InitSortingLayerSpine() { }

    protected virtual void ReleaseAttack()
    {
        PlaySound(soundAttack);
    }

    protected virtual void Patrol()
    {
        if (state == EnemyState.Patrol)
        {
            CheckAllowMovePatrol();

            if (isNotAllowMoveForward == false)
            {
                if (Mathf.Abs(transform.position.x - destinationMove.x) > 0.1f)
                {
                    Move();
                }
                else
                {
                    SwitchState(EnemyState.Idle);
                }
            }
            else
            {
                isRunning = false;
                SetDestinationPatrol(isMoveForward: false);
            }
        }
    }

    protected virtual void CheckAllowAttackTarget()
    {
        if (target != null)
        {
            Vector3 source = BodyCenterPoint.position;
            Vector3 destination = target.BodyCenterPoint.position;
            destination.y = source.y;
            isAllowAttackTarget = !Physics2D.Linecast(source, destination, layerMaskCheckObstacle);
        }
    }

    protected virtual void CheckAllowMoveForwardToTarget()
    {
        Vector2 v = frontCheckPoint.position + frontCheckPoint.right * 0.15f;
        bool isObstacleFront = Physics2D.Linecast(frontCheckPoint.position, v, StaticValue.LAYER_OBSTACLE);
        bool isGroundFront = Physics2D.Linecast(frontCheckPoint.position, groundCheckPoint.position, layerMaskCheckObstacle);

        if (isRunPassArea)
        {
            isNotAllowMoveForward = isObstacleFront;
        }
        else
        {
            isNotAllowMoveForward = isObstacleFront || !isGroundFront;
        }
    }

    protected virtual void CheckAllowMovePatrol()
    {
        Vector2 v = frontCheckPoint.position + frontCheckPoint.right * 0.15f;
        bool isObstacleFront = Physics2D.Linecast(frontCheckPoint.position, v, layerMaskCheckObstacle);
        Debug.DrawLine(frontCheckPoint.position, v, Color.red);
        bool isGroundFront = Physics2D.Linecast(frontCheckPoint.position, groundCheckPoint.position, layerMaskCheckObstacle);
        Debug.DrawLine(frontCheckPoint.position, groundCheckPoint.position, Color.green);

        isNotAllowMoveForward = isObstacleFront || !isGroundFront;
    }

    protected virtual bool IsTargetInCloseRange()
    {
        if (target != null)
        {
            float distance = Mathf.Abs(transform.position.x - target.transform.position.x);
            return distance <= closeUpRange;
        }

        return false;
    }

    protected virtual void CancelCombat()
    {
        target = null;
        SwitchState(EnemyState.Idle);
    }

    protected virtual void StartChasingTarget()
    {
        target = null;

        Vector3 v = transform.position;
        v.x = IsFacingRight ? v.x + 5f : v.x - 5f;
        SetDestinationMove(v);

        isRunning = true;
        SwitchState(EnemyState.Patrol);
    }

    protected virtual void GetCloseToTarget()
    {
        if (canMove && target != null)
        {
            if (flagGetCloseToTarget == false)
                return;

            Vector2 v = frontCheckPoint.position + frontCheckPoint.right * 0.15f;
            bool isObstacleFront = Physics2D.Linecast(frontCheckPoint.position, v, layerMaskCheckJump);
            bool isGroundFront = Physics2D.Linecast(frontCheckPoint.position, groundCheckPoint.position, layerMaskCheckObstacle);

            if (isObstacleFront)
            {
                if (canJump)
                {
                    if (flagJumpPassObstacle == false && isGroundFront)
                    {
                        flagJumpPassObstacle = true;
                        rigid.AddForce(jumpForce, ForceMode2D.Impulse);
                        StartCoroutine(DelayAction(() => { flagJumpPassObstacle = false; }, StaticValue.waitHalfSec));
                    }

                    Move();
                }
                else
                {
                    if (flagMove)
                    {
                        flagMove = false;
                        PlayAnimationIdle();
                    }
                }
            }
            else if (flagJumpPassObstacle == false)
            {
                bool close = Mathf.Abs(target.transform.position.x - transform.position.x) < 0.3f;

                if (isNotAllowMoveForward || close)
                {
                    if (flagMove)
                    {
                        flagMove = false;
                        PlayAnimationIdle();
                    }
                }
                else
                {
                    if (flagMove == false)
                    {
                        flagMove = true;
                        PlayAnimationMoveFast();
                    }

                    Move();
                }
            }
        }
    }

    protected virtual void ReadyToAttack()
    {
        isReadyAttack = true;
    }

    protected virtual void SetColliderLayers(bool isActive)
    {
        if (bodyCollider != null)
            bodyCollider.gameObject.layer = isActive ? StaticValue.LAYER_BODY_ENEMY : StaticValue.LAYER_DEFAULT;

        if (footCollider != null)
            footCollider.gameObject.layer = isActive ? StaticValue.LAYER_FOOT_ENEMY : StaticValue.LAYER_DEFAULT;
    }

    protected virtual void SetCloseRange()
    {
        if (nearSensor != null)
        {
            closeUpRange = Random.Range(1.5f, nearSensor.col.radius);
            nearSensor.col.radius = closeUpRange;
        }
    }

    protected virtual void InitPatrolPoint()
    {
        Vector3 v = transform.position;
        v.x = IsFacingRight ? v.x + 0.5f : v.x - 0.5f;
        SetDestinationMove(v);
    }

    protected virtual void SetDestinationPatrol(bool isMoveForward)
    {
        float randomDistance = Random.Range(0.5f, 3f);

        Vector3 pos = transform.position;

        if (isMoveForward)
        {
            if (IsFacingRight)
            {
                pos.x += randomDistance;
            }
            else
            {
                pos.x -= randomDistance;
            }
        }
        else
        {
            if (IsFacingRight)
            {
                pos.x -= randomDistance;
            }
            else
            {
                pos.x += randomDistance;
            }
        }

        SetDestinationMove(pos);
    }

    protected virtual List<ItemDropData> GetItemDrop()
    {
        List<ItemDropData> items = new List<ItemDropData>();

        int count = itemDropList.Count - 1;
        float percentRemaining = 100f;
        bool isDropped = false;

        while (count >= 0 && isDropped == false)
        {
            ItemDropData data = itemDropList[count];
            float rate = Mathf.Clamp01(data.dropRate / percentRemaining);
            int random = Random.Range(1, 101);

            if (Mathf.RoundToInt(rate * 100f) >= random)
            {
                isDropped = true;
                items.Add(data);
            }

            percentRemaining -= data.dropRate;
            percentRemaining = Mathf.Clamp(percentRemaining, 0f, 100f);
            count--;
        }

        // Coin
        if (bounty > 0)
        {
            ItemDropData coin = new ItemDropData(ItemDropType.Coin, bounty, 100f);
            items.Add(coin);
        }

        return items;
    }

    protected virtual void FadeIn()
    {
        DOTween.To(AlphaSetter, 0f, 1f, 2.5f).OnComplete(FadeInDone);
    }

    protected virtual void FadeInDone() { }

    protected virtual void ResetAim()
    {
        if (aimPoint != null)
            aimPoint.parent.localRotation = Quaternion.identity;

        if (aimBone != null)
            aimBone.transform.position = aimPoint.position;
    }

    public virtual BaseEnemy GetFromPool() { return null; }

    public virtual void ActiveSensor(bool isActive)
    {
        if (farSensor != null)
            farSensor.gameObject.SetActive(isActive);

        if (nearSensor != null)
            nearSensor.gameObject.SetActive(isActive);
    }

    public virtual void OnUnitGetInFarSensor(BaseUnit unit)
    {
        SetTarget(unit);

        if (Vector2.Distance(target.transform.position, BodyCenterPoint.position) > nearSensor.col.radius)
        {
            if (canMove)
            {
                flagGetCloseToTarget = true;
                PlayAnimationMoveFast();
            }
            else
            {
                PlayAnimationIdle();
            }
        }
    }

    public virtual void OnUnitGetOutFarSensor(BaseUnit unit)
    {
        if (canMove)
        {
            farSensor.gameObject.SetActive(false);
            StartCoroutine(DelayAction(() =>
            {
                farSensor.gameObject.SetActive(true);
                flagGetCloseToTarget = false;
                StartChasingTarget();
            },

            StaticValue.waitHalfSec));
        }
        else
        {
            CancelCombat();
        }
    }

    public virtual void OnUnitGetInNearSensor(BaseUnit unit)
    {
        if (canMove)
        {
            flagGetCloseToTarget = false;
            PlayAnimationIdle();
            StopMoving();
        }

        if (nearbyVictims.Contains(unit) == false)
            nearbyVictims.Add(unit);
    }

    public virtual void OnUnitGetOutNearSensor(BaseUnit unit)
    {
        if (canMove)
        {
            nearSensor.gameObject.SetActive(false);
            StartCoroutine(DelayAction(() =>
            {
                nearSensor.gameObject.SetActive(true);
                flagGetCloseToTarget = true;
                flagMove = true;
                PlayAnimationMoveFast();
            },

            StaticValue.waitHalfSec));
        }

        if (nearbyVictims.Contains(unit))
            nearbyVictims.Remove(unit);
    }

    #endregion


    #region SPINE & ANIMATION

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventShoot) == 0)
        {
            ReleaseAttack();
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(0, 0);
            skeletonAnimation.ClearState();
            Deactive();
        }
    }

    protected virtual void PlayAnimationShoot(int trackIndex = 1)
    {
        TrackEntry track = null;

        if (flagGetCloseToTarget)
        {
            track = skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
        }
        else
        {
            track = skeletonAnimation.AnimationState.SetAnimation(0, shoot, false);
        }

        track.AttachmentThreshold = 1f;
        track.MixDuration = 0f;
        TrackEntry empty = skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
        empty.AttachmentThreshold = 1f;
    }

    protected virtual void PlayAnimationMeleeAttack()
    {
        skeletonAnimation.AnimationState.SetAnimation(1, meleeAttack, false);
    }

    protected virtual void PlayAnimationThrow()
    {
        skeletonAnimation.AnimationState.SetAnimation(1, throwGrenade, false);
    }

    protected virtual void PlayAnimationDie()
    {
        int index = Random.Range(0, dieAnimationNames.Count);
        string animDie = dieAnimationNames[index];

        skeletonAnimation.AnimationState.SetAnimation(0, animDie, false);
    }

    public virtual void PlayAnimationIdle()
    {
        TrackEntry track = skeletonAnimation.AnimationState.GetCurrent(0);

        if (track == null || string.Compare(track.animation.name, idle) != 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
        }
    }

    public virtual void PlayAnimationMove()
    {
        TrackEntry track = skeletonAnimation.AnimationState.GetCurrent(0);

        if (track == null || string.Compare(track.animation.name, move) != 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, move, true);
        }
    }

    public virtual void PlayAnimationMoveFast()
    {
        TrackEntry track = skeletonAnimation.AnimationState.GetCurrent(0);

        if (string.IsNullOrEmpty(moveFast))
        {
            if (track == null || string.Compare(track.animation.name, move) != 0)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, move, true);
            }
        }
        else
        {
            if (track == null || string.Compare(track.animation.name, moveFast) != 0)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, moveFast, true);
            }
        }
    }

    #endregion


    protected void GetRandomPatrolPoint()
    {
        int randomDirection = Random.Range(0, 2);

        bool isMoveForward = randomDirection < 1;
        SetDestinationPatrol(isMoveForward);
    }

    protected virtual void TrackAimPoint()
    {
        if (target)
        {
            //bool isAim = Mathf.Abs(transform.position.x - target.transform.position.x) >= 0.7f;
            ActiveAim(true);
        }
        else
        {
            ActiveAim(false);
        }
    }

    protected virtual void ActiveAim(bool isActive)
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
            isReadyAttack = false;
            ResetAim();
        }
    }

    protected virtual void UpdateTransformPoints()
    {
        Vector3 v = groupTransformPoints.localEulerAngles;
        v.y = skeletonAnimation.Skeleton.flipX ? 180f : 0f;
        groupTransformPoints.localEulerAngles = v;
    }

    protected void AlphaSetter(float newValue)
    {
        SkeletonAnimation[] skeletons = GetComponentsInChildren<SkeletonAnimation>();

        for (int i = 0; i < skeletons.Length; i++)
        {
            skeletons[i].skeleton.a = newValue;
        }

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            Color c = renderers[i].color;
            c.a = newValue;
            renderers[i].color = c;
        }
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

    private IEnumerator CoroutineHideHealthBar()
    {
        yield return StaticValue.waitTwoSec;
        ActiveHealthBar(false);
    }

    public void DelayTargetPlayer()
    {
        this.StartDelayAction(() =>
        {
            SetTarget(GameController.Instance.Player);
            flagGetCloseToTarget = true;
            PlayAnimationMoveFast();
        }, 0.5f);
    }

    public void SetDestinationMove(Vector2 destination)
    {
        destinationMove = destination;
    }
}
