using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class BossVenom : BaseEnemy
{
    [Header("BOSS VENOM PROPERTIES")]
    public BulletPoisonBossVenom bulletPrefab;
    public PoisonTrap poisonTrapPrefab;
    public Collider2D head;
    public LaserBossVenom laser;
    [SpineAnimation]
    public string idleToLaser, idleToShoot, shootToIdle, shootLaser, shootPoison;
    [SpineEvent]
    public string eventShootLaser, eventShootPoison1, eventShootPoison2, eventShootPoison3;
    public AudioClip soundChangeState1, soundChangeState2, soundShootPoison, soundLaser;
    public Transform[] poisonFirePoints;

    private bool flagLaser;
    private bool flagPoison;
    private bool isScanningLaser;
    private bool isPingPongLaser;
    private Vector2 defaultAimPointPosition;
    private float timerScanLaser;
    private WaitForSeconds delayChangeAction;
    private WaitForSeconds delayShoot;
    private Transform pingPointLaser;
    private Transform pongPointLaser;

    public Transform FurthestLaserPoint { get; set; }
    public Transform NearestLaserPoint { get; set; }

    protected override void Awake()
    {
        base.Awake();

        defaultAimPointPosition = aimPoint.position;
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(DelayAction(AppearDone, StaticValue.waitOneSec));
        delayChangeAction = new WaitForSeconds(0.3f);
        delayShoot = new WaitForSeconds(((SO_BossVenomStats)baseStats).DelayShootTime);

        EventDispatcher.Instance.RegisterListener(EventID.LaserPoisonHitGround, (sender, param) => SpawnPoisonTrap((Vector2)param));
    }

    protected override void Update()
    {
        aimBone.transform.position = aimPoint.position;

        if (isReadyAttack)
        {
            Attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == StaticValue.LAYER_GROUND)
        {
            if (isDead)
            {
                EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
                StartCoroutine(DelayAction(Deactive, StaticValue.waitOneSec));
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_VENOM, level);
        baseStats = Resources.Load<SO_BossVenomStats>(path);
    }

    protected override void Attack()
    {
        ScanLaser();

        if (flagLaser)
        {
            flagLaser = false;
            skeletonAnimation.AnimationState.SetAnimation(0, idleToLaser, false);

            SoundManager.Instance.PlaySfx(soundLaser);
        }
        else if (flagPoison)
        {
            flagPoison = false;
            skeletonAnimation.AnimationState.SetAnimation(0, idleToShoot, false);
        }
    }

    protected override void StartDie()
    {
        base.StartDie();

        laser.gameObject.SetActive(false);
    }

    public override void Renew()
    {
        isDead = false;

        LoadScriptableObject();
        stats.Init(baseStats);

        isFinalBoss = true;
        head.enabled = false;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        laser.gameObject.SetActive(false);
        isEffectMeleeWeapon = false;
        isReadyAttack = false;
        transform.parent = null;
        UpdateTransformPoints();
        UpdateHealthBar();
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventShootLaser) == 0)
        {
            laser.gameObject.SetActive(true);
            StartCoroutine(DelayAction(() =>
            {
                isScanningLaser = true;
                pingPointLaser = aimPoint.position.x > target.transform.position.x ? FurthestLaserPoint : NearestLaserPoint;
                pongPointLaser = aimPoint.position.x > target.transform.position.x ? NearestLaserPoint : FurthestLaserPoint;
            },
            StaticValue.waitHalfSec));
        }

        if (string.Compare(e.Data.Name, eventShootPoison1) == 0)
            ReleasePoisonBullet(poisonFirePoints[0]);

        if (string.Compare(e.Data.Name, eventShootPoison2) == 0)
            ReleasePoisonBullet(poisonFirePoints[1]);

        if (string.Compare(e.Data.Name, eventShootPoison3) == 0)
            ReleasePoisonBullet(poisonFirePoints[2]);
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (string.Compare(entry.animation.name, idleToLaser) == 0)
            skeletonAnimation.AnimationState.SetAnimation(0, shootLaser, false);

        if (string.Compare(entry.animation.name, idleToShoot) == 0)
            StartCoroutine(CoroutineReleasePoisonBullet());

        if (string.Compare(entry.animation.name, shootToIdle) == 0)
        {
            PlayAnimationIdle();
            ResetAim();
            StartCoroutine(DelayAction(() =>
            {
                ActiveLaser();
            },
            delayChangeAction));
        }

        if (dieAnimationNames.Contains(entry.animation.name))
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.hudBoss.SetIconBoss(id);
        UIController.Instance.hudBoss.UpdateHP(HpPercent);
    }

    protected override void ResetAim()
    {
        aimPoint.parent.rotation = Quaternion.identity;
        aimPoint.position = defaultAimPointPosition;
    }

    private IEnumerator CoroutineReleasePoisonBullet()
    {
        int count = 0;
        int shootTimes = HpPercent > 0.5f ? ((SO_BossVenomStats)baseStats).ShootTimes : ((SO_BossVenomStats)baseStats).RageShootTimes;

        while (count < shootTimes)
        {
            SoundManager.Instance.PlaySfx(soundShootPoison);
            aimPoint.position = target.transform.position;
            skeletonAnimation.AnimationState.SetAnimation(0, shootPoison, false);
            count++;
            yield return delayShoot;
        }

        skeletonAnimation.AnimationState.SetAnimation(0, shootToIdle, false);
    }

    private void ReleasePoisonBullet(Transform point)
    {
        BulletPoisonBossVenom bullet = PoolingController.Instance.poolBulletPoisonBossVenom.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPoisonBossVenom;
        }

        float damage = HpPercent > 0.5f ? baseStats.Damage : ((SO_BossVenomStats)baseStats).RageDamage;
        float bulletSpeed = HpPercent > 0.5f ? baseStats.BulletSpeed : ((SO_BossVenomStats)baseStats).RageBulletSpeed;

        AttackData atkData = new AttackData(this, damage);
        bullet.Active(atkData, point, bulletSpeed, PoolingController.Instance.groupBullet);
    }

    private void AppearDone()
    {
        ReadyToAttack();
        head.enabled = true;
        ActiveLaser();

        EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
    }

    private void ScanLaser()
    {
        if (isScanningLaser == false)
            return;

        if (timerScanLaser < 1.5f)
        {
            timerScanLaser += Time.deltaTime;
            aimPoint.position = Vector2.MoveTowards(aimPoint.position, pingPointLaser.position, 10f * Time.deltaTime);
        }
        else
        {
            if (isPingPongLaser)
            {
                if (timerScanLaser < 3f)
                {
                    timerScanLaser += Time.deltaTime;
                    aimPoint.position = Vector2.MoveTowards(aimPoint.position, pongPointLaser.position, 10f * Time.deltaTime);
                }
                else
                {
                    StopLaser();
                }
            }
            else
            {
                StopLaser();
            }
        }
    }

    private void StopLaser()
    {
        timerScanLaser = 0;
        isScanningLaser = false;

        if (laser.hit.collider != null && laser.hit.collider.gameObject.layer == StaticValue.LAYER_GROUND)
        {
            SpawnPoisonTrap(laser.hit.point);
        }

        laser.gameObject.SetActive(false);
        ResetAim();
        ActiveShootPoison();
    }

    private void SpawnPoisonTrap(Vector2 position)
    {
        PoisonTrap trap = PoolingController.Instance.poolPoisonTrap.New();

        if (trap == null)
        {
            trap = Instantiate(poisonTrapPrefab) as PoisonTrap;
        }

        trap.Active(position);
    }

    private void ActiveLaser()
    {
        if (isDead)
            return;

        DebugCustom.Log("Active laser");
        isReadyAttack = false;
        ResetAim();
        aimPoint.position = target.transform.position;

        Vector2 v = target.transform.position;
        v.x = Mathf.Clamp(v.x, FurthestLaserPoint.position.x, NearestLaserPoint.position.x);
        aimPoint.position = v;

        timerScanLaser = 0;
        isPingPongLaser = HpPercent < 0.5f;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagLaser = true;
            flagPoison = false;

            SoundManager.Instance.PlaySfx(soundChangeState1);
        },

        delayChangeAction));
    }

    private void ActiveShootPoison()
    {
        if (isDead)
            return;

        DebugCustom.Log("Active shoot poison");
        isReadyAttack = false;
        ResetAim();
        aimPoint.position = target.transform.position;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagLaser = false;
            flagPoison = true;

            SoundManager.Instance.PlaySfx(soundChangeState2);
        },

        delayChangeAction));
    }
}
