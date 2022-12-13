using UnityEngine;
using System.Collections;
using Spine;

public class EnemyTank : BaseEnemy
{
    [Header("ENEMY TANK PROPERTIES")]
    public BaseBullet bulletPrefab;
    public BaseMuzzle muzzlePrefab;
    public BaseMuzzle dustMuzzlePrefab;
    public Transform firePoint;
    public Transform muzzlePoint;

    protected BaseMuzzle muzzle;
    protected BaseMuzzle dustMuzzle;


    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_TANK, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
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

            CheckAllowAttackTarget();

            if (isAllowAttackTarget)
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationShoot();
                    PlaySound(soundAttack);
                    ReleaseAttack();
                }
            }
        }
    }

    public override void TakeDamage(AttackData attackData)
    {
        base.TakeDamage(attackData);

        if (isDead)
        {
            if (attackData.weapon == WeaponType.Grenade)
            {
                EventDispatcher.Instance.PostEvent(EventID.KillTankByGrenade);
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyTank);
    }

    protected override void ReleaseAttack()
    {
        BulletTank bullet = PoolingController.Instance.poolBulletTank.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletTank;
        }

        AttackData atkData = new AttackData(this, baseStats.Damage);

        bullet.Active(atkData, firePoint, 5f, PoolingController.Instance.groupBullet);
        ActiveMuzzle();
    }

    protected override void StartDie()
    {
        base.StartDie();

        EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    protected override void SetColliderLayers(bool isActive)
    {
        if (bodyCollider != null)
        {
            bodyCollider.gameObject.layer = isActive ? StaticValue.LAYER_VEHICLE_ENEMY : StaticValue.LAYER_DEFAULT;
        }

        if (footCollider != null)
        {
            footCollider.gameObject.layer = isActive ? StaticValue.LAYER_VEHICLE_ENEMY : StaticValue.LAYER_DEFAULT;
        }
    }

    protected override void InitPatrolPoint()
    {
        Vector3 v = transform.position;
        v.x = IsFacingRight ? v.x + 2f : v.x - 2f;
        SetDestinationMove(v);
    }

    protected override void SetDestinationPatrol(bool isMoveForward)
    {
        float randomDistance = Random.Range(2f, 4.5f);

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

    public override void Renew()
    {
        base.Renew();

        isEffectMeleeWeapon = false;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyTank unit = PoolingController.Instance.poolEnemyTank.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyTank;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyTank.Store(this);
    }

    protected void ActiveMuzzle()
    {
        if (muzzle == null)
        {
            muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
        }

        muzzle.Active();

        if (dustMuzzle == null)
        {
            dustMuzzle = Instantiate<BaseMuzzle>(dustMuzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
        }

        dustMuzzle.Active();
    }
}
