using UnityEngine;
using System.Collections;

public class GunEnemyBazooka : BaseGunEnemy
{
    public override void Attack(BaseEnemy attacker)
    {
        base.Attack(attacker);

        BulletBazooka bullet = PoolingController.Instance.poolBulletBazooka.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletBazooka;
        }

        bullet.Active(attacker.GetCurentAttackData(), firePoint, attacker.baseStats.BulletSpeed, PoolingController.Instance.groupBullet);
        bullet.SetTarget(attacker.target.BodyCenterPoint);
    }
}
