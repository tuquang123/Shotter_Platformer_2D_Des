using UnityEngine;
using System.Collections;

public class Shotgun : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_SHOTGUN, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletShotgun bullet = PoolingController.Instance.poolBulletShotgun.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletShotgun;
        }

        bullet.Active(attackData, firePoint, bulletSpeed);
        ActiveMuzzle();
        CameraFollow.Instance.AddShake(0.15f, 0.2f);
    }
}
