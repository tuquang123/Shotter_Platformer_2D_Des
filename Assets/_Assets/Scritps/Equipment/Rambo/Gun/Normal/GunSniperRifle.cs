using UnityEngine;
using System.Collections;

public class GunSniperRifle : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_SNIPER_RIFLE, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletSniperRifle bullet = PoolingController.Instance.poolBulletSniperRifle.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletSniperRifle;
        }

        bullet.Active(attackData, firePoint, bulletSpeed);
        ActiveMuzzle();
    }
}
