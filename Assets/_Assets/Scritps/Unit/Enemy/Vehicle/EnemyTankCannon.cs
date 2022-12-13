using UnityEngine;
using System.Collections;

public class EnemyTankCannon : BaseEnemy
{
    [Header("ENEMY TANK PROPERTIES")]
    public BaseBullet bulletPrefab;
    public BaseMuzzle muzzlePrefab;
    public BaseMuzzle dustMuzzlePrefab;
    public Transform firePoint;
    public Transform muzzlePoint;
    public Vector2 fireDirection;

    protected BaseMuzzle muzzle;
    protected BaseMuzzle dustMuzzle;


    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_TANK_CANNON, level);
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

    public override BaseEnemy GetFromPool()
    {
        EnemyTankCannon unit = PoolingController.Instance.poolEnemyTankCannon.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyTankCannon;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyTankCannon.Store(this);
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyTank);
    }

    protected override void ReleaseAttack()
    {
        BulletTankCannon cannon = PoolingController.Instance.poolBulletTankCannon.New();

        if (cannon == null)
        {
            cannon = Instantiate(bulletPrefab) as BulletTankCannon;
        }

        AttackData atkData = new AttackData(this, baseStats.Damage, radiusDealDamage: 1f);

        Vector2 v = fireDirection;
        v.x = IsFacingRight ? v.x : -v.x;

        cannon.Active(atkData, firePoint, target.transform, v);
        ActiveMuzzle();
    }

    protected void ActiveMuzzle()
    {
        if (muzzle == null)
        {
            muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.transform);
        }

        muzzle.Active();

        if (dustMuzzle == null)
        {
            dustMuzzle = Instantiate<BaseMuzzle>(dustMuzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.transform);
        }

        dustMuzzle.Active();
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
}
