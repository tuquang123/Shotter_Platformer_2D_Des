using UnityEngine;
using System.Collections;
using Spine;

public class EnemyBomber : BaseEnemy
{
    [Header("ENEMY BOMBER PROPERTIES")]
    public Bomb bombPrefab;
    public Transform bombReleasePoint;
    public bool isFromLeft;

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
        TrackOutOfScreen();

        if (isDead == false)
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
                SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
                Deactive();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_BOMBER, level);
        baseStats = Resources.Load<SO_EnemyHasProjectileStats>(path);
    }

    protected override void Attack()
    {
        Vector2 v = isFromLeft ? Vector2.right : Vector2.left;
        transform.Translate(v * stats.MoveSpeed * Time.deltaTime);

        float currentTime = Time.time;

        if (currentTime - lastTimeAttack > stats.AttackRate)
        {
            lastTimeAttack = currentTime;
            PlayAnimationThrow();
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
        if (string.Compare(entry.animation.name, throwGrenade) == 0)
        {
            PlaySound(soundAttack);
            ReleaseBomb();
        }

        if (dieAnimationNames.Contains(entry.animation.name))
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.AddForce(Random.onUnitSphere * 10000f);
            rigid.AddTorque(30000f);
        }
    }

    protected override void PlayAnimationThrow()
    {
        TrackEntry track = skeletonAnimation.AnimationState.SetAnimation(1, throwGrenade, false);
        track.TimeScale = 3f;
        track.AttachmentThreshold = 1f;
        track.MixDuration = 0f;
        TrackEntry empty = skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
        empty.AttachmentThreshold = 1f;
    }

    public override void Renew()
    {
        base.Renew();

        rigid.bodyType = RigidbodyType2D.Kinematic;
        skeletonAnimation.Skeleton.FlipX = !isFromLeft;
        transform.rotation = Quaternion.identity;
        UpdateTransformPoints();
        EnableAudioMove(true);
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyBomber unit = PoolingController.Instance.poolEnemyBomber.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyBomber;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        EnableAudioMove(false);
        PoolingController.Instance.poolEnemyBomber.Store(this);
    }

    private void ReleaseBomb()
    {
        Bomb missile = PoolingController.Instance.poolBulletBomb.New();

        if (missile == null)
        {
            missile = Instantiate(bombPrefab) as Bomb;
        }

        float bombDamage = ((SO_EnemyHasProjectileStats)baseStats).ProjectileDamage;
        float bombRadius = ((SO_EnemyHasProjectileStats)baseStats).ProjectileDamageRadius;
        AttackData atkData = new AttackData(this, bombDamage, bombRadius);

        missile.Active(atkData, bombReleasePoint, 0f, PoolingController.Instance.groupBullet);
    }

    private void TrackOutOfScreen()
    {
        bool isOutScreen;

        if (isFromLeft)
        {
            isOutScreen = transform.position.x - 2f > CameraFollow.Instance.right.position.x;
        }
        else
        {
            isOutScreen = transform.position.x + 2f < CameraFollow.Instance.left.position.x;
        }

        if (isOutScreen)
            Deactive();
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
