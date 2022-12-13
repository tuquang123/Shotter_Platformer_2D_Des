using UnityEngine;
using System.Collections;

public class GunAWP : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_AWP, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletAWP bullet = PoolingController.Instance.poolBulletAWP.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletAWP;
        }

        bullet.Active(attackData, firePoint, bulletSpeed);
        ActiveMuzzle();
    }
}
