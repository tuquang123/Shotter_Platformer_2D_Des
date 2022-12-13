using UnityEngine;
using System.Collections;

public class GunEnemyPistol : BaseGunEnemy
{
    public override void Attack(BaseEnemy attacker)
    {
        base.Attack(attacker);

        BulletPistol bullet = PoolingController.Instance.poolBulletPistol.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPistol;
        }

        bullet.Active(attacker.GetCurentAttackData(), firePoint, attacker.baseStats.BulletSpeed, PoolingController.Instance.groupBullet);
    }
}
