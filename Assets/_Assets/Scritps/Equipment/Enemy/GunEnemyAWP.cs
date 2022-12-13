using UnityEngine;
using System.Collections;

public class GunEnemyAWP : BaseGunEnemy
{
    public SniperLaser laserAim;

    public void ActiveLaserAim(bool isActive)
    {
        laserAim.gameObject.SetActive(isActive);
    }

    public override void Attack(BaseEnemy attacker)
    {
        base.Attack(attacker);

        BulletSniper bullet = PoolingController.Instance.poolBulletSniper.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletSniper;
        }

        bullet.Active(attacker.GetCurentAttackData(), firePoint, attacker.baseStats.BulletSpeed, PoolingController.Instance.groupBullet);
    }
}
