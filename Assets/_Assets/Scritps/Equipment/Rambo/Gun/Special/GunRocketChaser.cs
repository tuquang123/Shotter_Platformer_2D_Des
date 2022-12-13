using UnityEngine;
using System.Collections;

public class GunRocketChaser : BaseGun
{
    public Transform rocketReadyPosition;


    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_ROCKET_CHASER, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletRocketChaser bullet = PoolingController.Instance.poolBulletRocketChaser.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletRocketChaser;
        }

        attackData.radiusDealDamage = ((SO_GunRocketChaserStats)baseStats).RadiusDealDamage;
        bullet.Active(attackData, firePoint, rocketReadyPosition.position, bulletSpeed);

        ActiveMuzzle();
    }
}
