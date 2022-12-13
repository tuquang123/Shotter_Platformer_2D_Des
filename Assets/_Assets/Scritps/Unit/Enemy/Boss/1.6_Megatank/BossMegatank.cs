using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;
using DG.Tweening;

public class BossMegatank : BaseEnemy
{
    [Header("BOSS MEGATANK PROPERTIES")]
    public BulletPlasma bulletPlasmaPrefab;
    public RocketBossMegatank rocketPrefab;
    public BossMegatankColliderWheel colWheel;
    public BaseMuzzle muzzlePlasmaPrefab;
    public BaseMuzzle dustMuzzlePrefab;

    public Transform aimPlasma;
    public Transform firePointPlasma;
    public Transform muzzlePointPlasma;
    public Transform defaultAimGunPosition;

    public Transform aimCannon;
    public Transform firePointCannon;
    public Transform muzzlePointCannon;
    public Vector2 cannonFireDirection;

    [SpineAnimation]
    public string cannonShoot, gunShoot, preGore, gore1, gore2, goreToIdle;
    public AudioClip soundPregore, soundGore, soundPlasma, soundCannon;

    private BaseMuzzle muzzlePlasma;
    private BaseMuzzle dustMuzzle;
    [SerializeField]
    private bool isMovingToBase = true;
    [SerializeField]
    private bool flagCannon = true;
    [SerializeField]
    private bool flagPlasma;
    private Vector2 destinationGore;
    private float countGunPlasma;
    private int countGoreTime;

    protected override void Awake()
    {
        base.Awake();

        EventDispatcher.Instance.RegisterListener(EventID.RocketMegatankHitPlayer, (sender, param) => OnCannonHitPlayer());
        EventDispatcher.Instance.RegisterListener(EventID.RocketMegatankMissPlayer, (sender, param) => OnCannonMissPlayer());
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            MoveToBase();

            if (isReadyAttack)
            {
                Attack();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_MEGATANK, level);
        baseStats = Resources.Load<SO_BossMegatankStats>(path);
    }

    protected override void Attack()
    {
        if (flagCannon)
        {
            UpdateDirection();
            flagCannon = false;
            skeletonAnimation.AnimationState.SetAnimation(1, cannonShoot, false);
        }
        else if (flagPlasma)
        {
            UpdateDirection();
            UpdateAimPlasma();

            if (countGunPlasma < ((SO_BossMegatankStats)baseStats).PlasmaDuration)
            {
                countGunPlasma += Time.deltaTime;

                float currentTime = Time.time;
                float attackRate = HpPercent > 0.5f ? stats.AttackRate : (1f / ((SO_BossMegatankStats)baseStats).RageAttackTimeSecond);
                if (currentTime - lastTimeAttack > attackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationGunPlasma();
                    ShootPlasma();
                }
            }
            else
            {
                ActiveCannon();
            }
        }
    }

    protected override void UpdateDirection()
    {
        if (isMovingToBase)
        {
            skeletonAnimation.Skeleton.flipX = (basePosition.x < transform.position.x);
        }
        else if (target != null)
        {
            skeletonAnimation.Skeleton.flipX = (target.transform.position.x < transform.position.x);
        }

        UpdateTransformPoints();
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            Deactive();
        }

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, cannonShoot) == 0)
        {
            ShootCannon(firePointCannon, target.transform);
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }

        if (string.Compare(entry.animation.name, preGore) == 0)
        {
            SoundManager.Instance.PlaySfx(soundGore);

            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            skeletonAnimation.AnimationState.SetAnimation(1, gore1, true);
            skeletonAnimation.AnimationState.SetAnimation(2, gore2, true);
            colWheel.gameObject.SetActive(true);
            destinationGore = target.transform.position;
            destinationGore.y = transform.position.y;

            CameraFollow.Instance.AddShake(0.15f, 1f);
            transform.DOMoveX(destinationGore.x, 1f).OnComplete(() =>
            {
                StopMoving();
                UpdateDirection();
                transform.position = destinationGore;
                skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
                skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
                colWheel.gameObject.SetActive(false);
                PlayAnimationIdle();

                if (HpPercent < 0.5f && countGoreTime < 2)
                {
                    countGoreTime++;
                    ActiveGore();
                }
                else
                {
                    countGoreTime = 0;
                    ActivePlasma();
                }
            });
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e) { }

    public override void Renew()
    {
        isDead = false;

        LoadScriptableObject();
        stats.Init(baseStats);

        isFinalBoss = true;
        canMove = true;
        isEffectMeleeWeapon = false;
        isReadyAttack = false;
        target = null;
        transform.parent = null;
        UpdateTransformPoints();
        UpdateHealthBar();
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.hudBoss.SetIconBoss(id);
        UIController.Instance.hudBoss.UpdateHP(HpPercent);
    }

    private void MoveToBase()
    {
        if (isMovingToBase)
        {
            isMovingToBase = false;
            PlayAnimationMove();

            transform.DOMove(basePosition, 3f).OnComplete(() =>
            {
                EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
                PrepareAttack();

            }).OnStart(() =>
            {
                CameraFollow.Instance.AddShake(0.3f, 3f);
            });
        }
    }

    private void UpdateAimPlasma()
    {
        if (target)
            aimPlasma.position = target.BodyCenterPoint.position;
    }

    private void PrepareAttack()
    {
        PlayAnimationIdle();
        StopMoving();
        ActiveCannon();
    }

    private void ShootPlasma()
    {
        BulletPlasma bullet = PoolingController.Instance.poolBulletPlasma.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPlasmaPrefab) as BulletPlasma;
        }

        float damage = HpPercent > 0.5f ? baseStats.Damage : ((SO_BossMegatankStats)baseStats).RageGunDamage;
        float bulletSpeed = HpPercent > 0.5f ? baseStats.BulletSpeed : ((SO_BossMegatankStats)baseStats).RageBulletSpeed;
        AttackData atkData = new AttackData(this, damage);
        bullet.Active(atkData, firePointPlasma, bulletSpeed, PoolingController.Instance.groupBullet);

        if (muzzlePlasma == null)
        {
            muzzlePlasma = Instantiate(muzzlePlasmaPrefab, muzzlePointPlasma.position, muzzlePointPlasma.rotation, muzzlePointPlasma.parent);
        }

        muzzlePlasma.Active();

        SoundManager.Instance.PlaySfx(soundPlasma);
    }

    private void ShootCannon(Transform startPoint, Transform endPoint)
    {
        RocketBossMegatank rocket = PoolingController.Instance.poolRocketBossMegatank.New();

        if (rocket == null)
        {
            rocket = Instantiate(rocketPrefab) as RocketBossMegatank;
        }

        float rocketDamage = HpPercent > 0.5f ? ((SO_BossMegatankStats)baseStats).RocketDamage : ((SO_BossMegatankStats)baseStats).RageRocketDamage;
        float rocketRadius = ((SO_BossMegatankStats)baseStats).RocketRadius;
        AttackData atkData = new AttackData(this, rocketDamage, rocketRadius);

        Vector2 v = cannonFireDirection;
        v.x = IsFacingRight ? v.x : -v.x;

        rocket.Active(atkData, startPoint, endPoint, v);

        if (dustMuzzle == null)
        {
            dustMuzzle = Instantiate(dustMuzzlePrefab, muzzlePointCannon.position, muzzlePointCannon.rotation, muzzlePointCannon.parent);
        }

        dustMuzzle.Active();

        SoundManager.Instance.PlaySfx(soundCannon);
    }

    private void PlayAnimationGunPlasma()
    {
        TrackEntry track = skeletonAnimation.AnimationState.SetAnimation(1, gunShoot, false);
        track.AttachmentThreshold = 1f;
        track.MixDuration = 0f;
        TrackEntry empty = skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
        empty.AttachmentThreshold = 1f;
    }

    private void TriggerAnimationPreGore()
    {
        if (string.Compare(skeletonAnimation.AnimationState.GetCurrent(0).animation.name, preGore) != 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, preGore, false);
            skeletonAnimation.AnimationState.SetAnimation(1, gore2, true);
            UpdateDirection();
        }
    }

    private void OnCannonHitPlayer()
    {
        if (isDead == false)
            ActivePlasma();
    }

    private void OnCannonMissPlayer()
    {
        if (isDead == false)
        {
            if (Mathf.Abs(transform.position.x - target.transform.position.x) <= 2f)
            {
                ActivePlasma();
            }
            else
            {
                ActiveGore();
            }
        }
    }

    private void ActiveCannon()
    {
        DebugCustom.Log("Active cannon");
        isReadyAttack = false;
        ResetAim();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagCannon = true;
            flagPlasma = false;
            countGunPlasma = 0;
        },

        StaticValue.waitOneSec));
    }

    private void ActivePlasma()
    {
        DebugCustom.Log("Active plasma");
        isReadyAttack = false;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagCannon = false;
            flagPlasma = true;
            countGunPlasma = 0;
        },

        StaticValue.waitOneSec));
    }

    private void ActiveGore()
    {
        DebugCustom.Log("Active gore");
        isReadyAttack = false;
        ResetAim();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagCannon = false;
            flagPlasma = false;

            skeletonAnimation.AnimationState.SetAnimation(0, preGore, false);
            skeletonAnimation.AnimationState.SetAnimation(1, gore2, true);
            UpdateDirection();

            SoundManager.Instance.PlaySfx(soundPregore);
        },

        StaticValue.waitOneSec));
    }
}
