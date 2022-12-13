using UnityEngine;
using System.Collections;
using Spine;

public class EnemySpider : BaseEnemy
{
    [Header("ENEMY SPIDER PROPERTIES")]
    public BaseBullet bulletPrefab;

    private bool flagShoot;

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
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_SPIDER, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void InitWeapon() { }

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

            GetCloseToTarget();
            CheckAllowAttackTarget();

            if (isAllowAttackTarget)
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationShoot();
                }
            }
        }
    }

    protected override void ReleaseAttack()
    {
        //base.ReleaseAttack();

        BulletSpider bullet = PoolingController.Instance.poolBulletSpider.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletSpider;
        }

        AttackData atkData = GetCurentAttackData();
        float speed = baseStats.BulletSpeed;

        bullet.Active(atkData, aimPoint, speed, PoolingController.Instance.groupBullet);
    }

    protected override void SetCloseRange()
    {
        if (nearSensor != null)
        {
            closeUpRange = nearSensor.col.radius;
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemySpider unit = PoolingController.Instance.poolEnemySpider.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemySpider;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemySpider.Store(this);
    }
}
