using UnityEngine;
using System.Collections;
using Spine;

public class EnemyFire : BaseEnemy
{
    [Header("ENEMY FIRE PROPERTIES")]
    public Fire fire;

    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.PlayerDie, (sender, param) =>
        {
            fire.Deactive();
        });
    }

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
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_FIRE, level);
        baseStats = Resources.Load<SO_EnemyFireStats>(path);
    }

    protected override void Attack()
    {
        if (state == EnemyState.Attack)
        {
            if (target == null || target.isDead)
            {
                CancelCombat();
                return;
            }

            GetCloseToTarget();
        }
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyFire);
    }

    protected override void SetCloseRange()
    {
        if (nearSensor != null)
        {
            closeUpRange = Random.Range(2f, 3f);
            nearSensor.col.radius = closeUpRange;
        }
    }

    protected override void PlayAnimationShoot(int trackIndex = 1)
    {
        skeletonAnimation.AnimationState.SetAnimation(trackIndex, shoot, true);
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyFire unit = PoolingController.Instance.poolEnemyFire.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyFire;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyFire.Store(this);
    }

    public override void OnUnitGetInFarSensor(BaseUnit unit)
    {
        SetTarget(unit);

        fire.Active();

        if (Vector2.Distance(target.transform.position, BodyCenterPoint.position) > nearSensor.col.radius)
        {
            if (canMove)
            {
                flagGetCloseToTarget = true;
                PlayAnimationMoveFast();
                PlayAnimationShoot(trackIndex: 1);
            }
            else
            {
                PlayAnimationShoot(trackIndex: 0);
            }
        }
        else
        {
            PlayAnimationShoot(trackIndex: 0);
        }
    }

    public override void OnUnitGetOutFarSensor(BaseUnit unit)
    {
        fire.Deactive();

        if (canMove)
        {
            farSensor.gameObject.SetActive(false);
            StartCoroutine(DelayAction(() =>
            {
                farSensor.gameObject.SetActive(true);
                flagGetCloseToTarget = false;
                StartChasingTarget();
                skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            },

            StaticValue.waitHalfSec));
        }
        else
        {
            CancelCombat();
        }
    }

    public override void OnUnitGetInNearSensor(BaseUnit unit)
    {
        if (canMove)
        {
            flagGetCloseToTarget = false;
            PlayAnimationIdle();
            StopMoving();
            PlayAnimationShoot(trackIndex: 0);
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }

        if (nearbyVictims.Contains(unit) == false)
            nearbyVictims.Add(unit);
    }

    public override void OnUnitGetOutNearSensor(BaseUnit unit)
    {
        if (canMove)
        {
            nearSensor.gameObject.SetActive(false);
            StartCoroutine(DelayAction(() =>
            {
                nearSensor.gameObject.SetActive(true);
                PlayAnimationMoveFast();
                PlayAnimationShoot(trackIndex: 1);
                flagGetCloseToTarget = true;
            },

            StaticValue.waitHalfSec));
        }

        if (nearbyVictims.Contains(unit))
            nearbyVictims.Remove(unit);
    }

    protected override void UpdateTransformPoints()
    {
        base.UpdateTransformPoints();

        Vector3 v = fire.fireEffect.localScale;
        v.x = IsFacingRight ? 1 : -1;
        fire.fireEffect.localScale = v;
    }
}
