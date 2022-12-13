using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class EnemyMonkey : BaseEnemy
{
    [Header("ENEMY MONKEY PROPERTIES")]
    public Collider2D colliderArm;
    public Transform healthBarLeft;
    public Transform healthBarRight;
    [SpineAnimation]
    public string punch, smash, idleNormal, idleRoar;
    [SpineEvent]
    public string eventPunch, eventSmash;
    [SerializeField]
    private bool flagAttack;

    public bool IsAttacking { get { return flagAttack; } }


    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            Idle();
            Patrol();
            Attack();
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_MONKEY, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void InitSortingLayerSpine() { }

    protected override void Attack()
    {
        if (state == EnemyState.Attack)
        {
            if (target == null || target.isDead)
            {
                CancelCombat();
                return;
            }

            if (flagAttack) return;

            GetCloseToTarget();

            if (flagGetCloseToTarget == false)
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    flagAttack = true;
                    colliderArm.enabled = true;
                    PlayAnimationMeleeAttack();
                }
            }
        }
    }

    protected override void SetCloseRange()
    {
        if (nearSensor != null)
        {
            if (nearSensor != null)
            {
                closeUpRange = Random.Range(0.7f, 1.2f);
                nearSensor.col.radius = closeUpRange;
            }
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventPunch) == 0
            || string.Compare(e.Data.Name, eventSmash) == 0)
        {
            if (isDead == false)
            {
                PlaySound(soundAttack);
            }
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, punch) == 0
            || string.Compare(entry.animation.name, smash) == 0)
        {
            flagAttack = false;
            colliderArm.enabled = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
        }
    }

    protected override void PlayAnimationMeleeAttack()
    {
        string animName = Random.Range(0, 2) == 0 ? punch : smash;
        skeletonAnimation.AnimationState.SetAnimation(1, animName, false);
    }

    public override void PlayAnimationIdle()
    {
        TrackEntry track = skeletonAnimation.AnimationState.GetCurrent(0);

        if (track == null || (string.Compare(track.animation.name, idleNormal) != 0 && string.Compare(track.animation.name, idleRoar) != 0))
        {
            string animName = idleNormal;

            if (target == null)
            {
                animName = Random.Range(0, 2) == 0 ? idleNormal : idleRoar;
            }

            skeletonAnimation.AnimationState.SetAnimation(0, animName, true);
        }
    }

    protected override void UpdateTransformPoints()
    {
        base.UpdateTransformPoints();

        healthBar.transform.parent.position = skeletonAnimation.Skeleton.flipX ? healthBarLeft.position : healthBarRight.position;
    }

    public override void Renew()
    {
        base.Renew();

        colliderArm.enabled = false;
        flagAttack = false;
        canJump = true;
    }

    public override void Active(EnemySpawnData spawnData)
    {
        base.Active(spawnData);

        canMove = true;
        canJump = true;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyMonkey unit = PoolingController.Instance.poolEnemyMonkey.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyMonkey;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyMonkey.Store(this);
    }
}

