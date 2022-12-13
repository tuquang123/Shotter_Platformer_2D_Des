using UnityEngine;
using System.Collections;
using Spine;

public class EnemyHelicopter : BaseEnemy
{
    [HideInInspector]
    public int indexMove = 0;
    public HomingMissile bombPrefab;
    public BaseMuzzle dustMuzzlePrefab;
    public Transform missileReleasePoint;

    private int indexRotate = -1;
    private bool isMovingToDestination;
    private float lastTimeIdle;
    private float timeIdle;
    private BaseMuzzle dustMuzzle;
    private AudioSource audioMove;
    private AudioClip soundMove;


    protected override void Awake()
    {
        base.Awake();

        audioMove = GetComponent<AudioSource>();
        soundMove = SoundManager.Instance.GetAudioClip(StaticValue.SOUND_SFX_PLANE_MOVE);
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
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
                SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
                Deactive();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_HELICOPTER, level);
        baseStats = Resources.Load<SO_EnemyHelicopterStats>(path);
    }

    protected override void Attack()
    {
        if (state == EnemyState.Attack)
        {
            if (target == null || target.isDead)
            {
                return;
            }
        }

        if (isMovingToDestination)
        {
            if (Mathf.Abs(destinationMove.x - transform.position.x) > 0.05f)
            {
                transform.position = Vector2.MoveTowards(transform.position, destinationMove, stats.MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = destinationMove;
                isMovingToDestination = false;
                EnableAudioMove(false);
                skeletonAnimation.transform.rotation = Quaternion.identity;
                StartCoroutine(DelayAction(ReadyToAttack, StaticValue.waitOneSec));
            }
        }
        else
        {
            if (isReadyAttack)
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeIdle > timeIdle)
                {
                    GetNextDestination();
                    skeletonAnimation.transform.rotation = Quaternion.Euler(0, 0, IsFacingRight ? -15f : 15f);
                    isReadyAttack = false;
                    isMovingToDestination = true;
                    EnableAudioMove(true);
                    PlayAnimationIdle();
                    return;
                }

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationShoot();
                    PlaySound(soundAttack);
                    ReleaseMissile();
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyFlying);
    }

    protected override void StartDie()
    {
        base.StartDie();

        EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.AddForce(Random.onUnitSphere * 10000f);
            rigid.AddTorque(30000f);
        }
    }

    protected override void UpdateDirection()
    {
        if (isMovingToDestination)
        {
            skeletonAnimation.Skeleton.flipX = (destinationMove.x < transform.position.x);
        }
        else
        {
            skeletonAnimation.Skeleton.flipX = (target.transform.position.x < transform.position.x);
        }

        UpdateTransformPoints();
    }

    public override void Renew()
    {
        base.Renew();

        isEffectMeleeWeapon = false;
        indexMove = 0;
        indexRotate = -1;
        timeIdle = Random.Range(5f, 7f);
        isMovingToDestination = true;
        EnableAudioMove(true);
        transform.rotation = Quaternion.identity;
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyHelicopter unit = PoolingController.Instance.poolEnemyHelicopter.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyHelicopter;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        EnableAudioMove(false);
        PoolingController.Instance.poolEnemyHelicopter.Store(this);
    }

    protected override void ReadyToAttack()
    {
        base.ReadyToAttack();

        lastTimeIdle = Time.time;
    }

    public void GetNextDestination()
    {
        Vector2 v = CameraFollow.Instance.GetNextDestination(this);
        SetDestinationMove(v);
    }

    private void ReleaseMissile()
    {
        SO_EnemyHelicopterStats stat = (SO_EnemyHelicopterStats)baseStats;

        for (int i = 0; i < stat.NumberOfProjectilePerShot; i++)
        {
            HomingMissile missile = PoolingController.Instance.poolHomingMissile.New();

            if (missile == null)
            {
                missile = Instantiate(bombPrefab) as HomingMissile;
            }

            float missileDamage = ((SO_EnemyHelicopterStats)baseStats).ProjectileDamage;
            float missileRadius = ((SO_EnemyHelicopterStats)baseStats).ProjectileDamageRadius;
            AttackData atkData = new AttackData(this, missileDamage, missileRadius);

            missile.Active(atkData, missileReleasePoint, stat.ProjectileSpeed, PoolingController.Instance.groupBullet);
            missile.Rotate(indexRotate);
            missile.SetTarget(target.transform);

            indexRotate++;
            if (indexRotate >= 2)
            {
                indexRotate = -1;
            }
        }

        ActiveMuzzle();
    }

    private void ActiveMuzzle()
    {
        if (dustMuzzle == null)
        {
            dustMuzzle = Instantiate<BaseMuzzle>(dustMuzzlePrefab, missileReleasePoint.position, missileReleasePoint.rotation, missileReleasePoint);
        }

        dustMuzzle.Active();
    }

    private void EnableAudioMove(bool isEnable)
    {
        if (isEnable)
        {
            if (audioMove.clip == null)
            {
                audioMove.clip = soundMove;
                audioMove.loop = true;
            }

            audioMove.Play();
        }
        else
        {
            if (audioMove.clip = soundMove)
            {
                audioMove.clip = null;
            }

            audioMove.Stop();
        }
    }
}
