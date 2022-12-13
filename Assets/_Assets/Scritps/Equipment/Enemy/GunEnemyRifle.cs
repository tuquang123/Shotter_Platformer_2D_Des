using UnityEngine;
using System.Collections;

public class GunEnemyRifle : BaseGunEnemy
{
    public override void Attack(BaseEnemy attacker)
    {
        base.Attack(attacker);

        BulletRifle bullet = PoolingController.Instance.poolBulletRifle.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletRifle;
        }

        bullet.Active(attacker.GetCurentAttackData(), firePoint, attacker.baseStats.BulletSpeed, PoolingController.Instance.groupBullet);
    }
}
