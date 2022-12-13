using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using DG.Tweening;
using System.Collections.Generic;

public class BossMegatron : BaseEnemy
{
    [Header("BOSS MEGATRON PROPERTIES")]
    public Collider2D head;
    public Collider2D foot;
    public BossMegatronColliderGround colliderCheckGround;
    public BulletBossMegatron bulletPrefab;
    public BaseMuzzle muzzlePrefab;
    public Transform muzzlePoint;
    public ParticleSystem dustMove;
    public GameObject effectWarningPoint;
    public GameObject effectTrailFire;
    [SpineAnimation]
    public string idleToShoot, jumpToIdle, jumpAttack, landing;
    [SpineSkin]
    public string skin25, skin50, skin100;
    [SpineEvent]
    public string eventFootStep, eventActiveTrail, eventFly;
    public AudioClip soundAppear, soundMove, soundShoot, soundPreSmash, soundSmash, soundJump, soundGrounded;

    private BaseMuzzle muzzle;
    private bool flagMovingEntrance = true;
    private bool flagSmash;
    private bool flagShoot;
    private bool flagJump;
    private int countShoot;
    private int totalBulletShoot;
    private int countSmash;
    private int totalSmash;

    public bool IsSmashing { get; set; }

    protected override void Awake()
    {
        base.Awake();

        EventDispatcher.Instance.RegisterListener(EventID.ShowInfoBossDone, (sender, param) => ViewInfoBossDone());
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            Entrance();

            if (isReadyAttack)
            {
                Attack();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_MEGATRON, level);
        baseStats = Resources.Load<SO_BossMegatronStats>(path);
    }

    protected override void Attack()
    {
        if (flagJump)
        {
            flagJump = false;
            PlayAnimationJump();
        }
        else if (flagShoot)
        {
            flagShoot = false;
            PlayAnimationPrepareShoot();
        }
        else if (flagSmash)
        {
            if (Mathf.Abs(target.transform.position.x - transform.position.x) > 3.5f)
            {
                PlayAnimationMove();
                Move();
            }
            else
            {
                flagSmash = false;
                StopMoving();
                PlayAnimationMeleeAttack();
                SoundManager.Instance.PlaySfx(soundPreSmash);
            }
        }
    }

    protected override void UpdateDirection()
    {
        if (target != null && !flagSmash && !flagShoot && !flagJump)
        {
            skeletonAnimation.Skeleton.flipX = (target.transform.position.x < transform.position.x);
        }

        UpdateTransformPoints();
    }

    public override void Renew()
    {
        isDead = false;

        LoadScriptableObject();
        stats.Init(baseStats);

        isFinalBoss = true;
        head.enabled = false;
        foot.enabled = false;
        canMove = true;
        isEffectMeleeWeapon = false;
        isReadyAttack = false;
        target = null;
        transform.parent = null;
        effectWarningPoint.transform.parent = null;
        UpdateTransformPoints();
        UpdateHealthBar();
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.hudBoss.SetIconBoss(id);
        UIController.Instance.hudBoss.UpdateHP(HpPercent);
    }

    public override void TakeDamage(AttackData attackData)
    {
        base.TakeDamage(attackData);

        string skin = defaultSkin;

        if (HpPercent < 0.5f && HpPercent > 0.25f)
        {
            skin = skin50;
        }
        else if (HpPercent < 0.25f)
        {
            skin = skin25;
        }

        if (string.Compare(defaultSkin, skin) != 0)
        {
            skeletonAnimation.Skeleton.SetSkin(skin);
        }
    }

    private void Entrance()
    {
        if (flagMovingEntrance)
        {
            flagMovingEntrance = false;

            transform.DOMove(basePosition, 0.4f).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                rigid.bodyType = RigidbodyType2D.Dynamic;
                skeletonAnimation.AnimationState.SetAnimation(1, jumpToIdle, false);
                CameraFollow.Instance.AddShake(1.2f, 1f);
                SoundManager.Instance.PlaySfx(soundGrounded, 10f);

                if (GameData.mode == GameMode.Campaign)
                {
                    StartCoroutine(DelayAction(() => { EventDispatcher.Instance.PostEvent(EventID.ShowInfoBossMegatron); }, StaticValue.waitOneSec));
                }
                else if (GameData.mode == GameMode.Survival)
                {
                    ViewInfoBossDone();
                }

            }).OnStart(() =>
            {
                rigid.bodyType = RigidbodyType2D.Kinematic;
                skeletonAnimation.AnimationState.SetAnimation(1, landing, false);
            });
        }
    }

    private void ViewInfoBossDone()
    {
        head.enabled = true;
        foot.enabled = true;
        ActiveShoot();

        EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
        SoundManager.Instance.PlaySfx(soundAppear);
    }

    private void Landing()
    {
        if (target == null)
            return;

        Vector2 v = target.transform.position;
        v.y = basePosition.y;
        v.x += Random.Range(-2f, 2f);
        v.x = Mathf.Clamp(v.x, CameraFollow.Instance.left.position.x + 2f, CameraFollow.Instance.right.position.x - 2f);

        Vector2 pos = transform.position;
        pos.x = v.x;
        transform.position = pos;

        effectWarningPoint.transform.position = v;
        effectWarningPoint.SetActive(true);

        transform.DOMove(v, 0.4f).SetDelay(0.85f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ActiveGroundCollider(false);
            rigid.bodyType = RigidbodyType2D.Dynamic;
            dustMove.Play();
            effectWarningPoint.SetActive(false);
            skeletonAnimation.AnimationState.SetAnimation(1, jumpToIdle, false);
            CameraFollow.Instance.AddShake(1.2f, 1f);
            SoundManager.Instance.PlaySfx(soundGrounded, 10f);

            if (Random.Range(0, 2) == 0)
            {
                ActiveSmash();
            }
            else
            {
                ActiveShoot();
            }

        }).OnStart(() =>
        {
            skeletonAnimation.AnimationState.SetAnimation(1, landing, false);
        });
    }

    private void ReleaseBullet()
    {
        BulletBossMegatron bullet = PoolingController.Instance.poolBulletBossMegatron.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletBossMegatron;
        }

        bullet.Active(GetCurentAttackData(), aimPoint, baseStats.BulletSpeed, PoolingController.Instance.groupBullet);

        SoundManager.Instance.PlaySfx(soundShoot);
    }

    private void ActiveJump()
    {
        isReadyAttack = false;
        StopMoving();
        PlayAnimationIdle();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagJump = true;
            flagSmash = false;
            flagShoot = false;
        },

        StaticValue.waitOneSec));
    }

    private void ActiveSmash()
    {
        countSmash = 0;
        totalSmash = Random.Range(1, 3);
        isReadyAttack = false;
        StopMoving();
        PlayAnimationIdle();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagJump = false;
            flagSmash = true;
            flagShoot = false;
        },

        StaticValue.waitOneSec));
    }

    private void ActiveShoot()
    {
        countShoot = 0;
        totalBulletShoot = Random.Range(1, 3);
        isReadyAttack = false;
        StopMoving();
        PlayAnimationIdle();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagJump = false;
            flagSmash = false;
            flagShoot = true;
        },

        StaticValue.waitOneSec));
    }

    private void ActiveGroundCollider(bool isActive)
    {
        colliderCheckGround.gameObject.SetActive(isActive);
    }


    #region SPINE & ANIMATIONS

    protected override void HandleAnimationStart(TrackEntry entry)
    {
        base.HandleAnimationStart(entry);

        if (string.Compare(entry.animation.name, shoot) == 0)
        {
            if (muzzle == null)
            {
                muzzle = Instantiate(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
            }

            muzzle.Active();
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            Deactive();
        }

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, idleToShoot) == 0)
        {
            PlayAnimationShoot();
        }

        if (string.Compare(entry.animation.name, shoot) == 0)
        {
            countShoot++;

            if (countShoot < totalBulletShoot)
            {
                flagShoot = true;
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    ActiveJump();
                }
                else
                {
                    ActiveSmash();
                }
            }
        }

        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            IsSmashing = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            SoundManager.Instance.PlaySfx(soundSmash);

            countSmash++;

            if (countSmash < totalSmash)
            {
                flagSmash = true;
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    ActiveJump();
                }
                else
                {
                    ActiveShoot();
                }
            }
        }

        if (string.Compare(entry.animation.name, jumpAttack) == 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(1, jumpToIdle, false);
        }

        if (string.Compare(entry.animation.name, jumpToIdle) == 0)
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventFootStep) == 0)
        {
            CameraFollow.Instance.AddShake(0.3f, 0.5f);
            dustMove.Play();

            SoundManager.Instance.PlaySfx(soundMove);
        }

        if (string.Compare(e.Data.Name, eventMeleeAttack) == 0)
        {
            CameraFollow.Instance.AddShake(0.3f, 0.5f);
        }

        if (string.Compare(e.Data.Name, eventActiveTrail) == 0)
        {
            effectTrailFire.SetActive(true);
        }

        if (string.Compare(e.Data.Name, eventShoot) == 0)
        {
            ReleaseBullet();
        }

        if (string.Compare(e.Data.Name, eventFly) == 0)
        {
            rigid.bodyType = RigidbodyType2D.Kinematic;

            CameraFollow.Instance.AddShake(0.5f, 0.5f);

            float y = basePosition.y + 8f;

            transform.DOMoveY(y, 2f).OnComplete(() =>
            {
                ActiveGroundCollider(true);
                effectTrailFire.SetActive(false);
                Landing();
            });

            SoundManager.Instance.PlaySfx(soundJump);
        }
    }

    protected override void PlayAnimationMeleeAttack()
    {
        base.PlayAnimationMeleeAttack();

        IsSmashing = true;
    }

    private void PlayAnimationPrepareShoot()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, idleToShoot, false);
    }

    private void PlayAnimationJump()
    {
        skeletonAnimation.AnimationState.SetAnimation(1, jumpAttack, false);
    }

    #endregion

}
