using UnityEngine;
using System.Collections;

public class GunScarH : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_SCAR_H, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletScarHGun bullet = PoolingController.Instance.poolBulletScarHGun.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletScarHGun;
        }

        bullet.Active(attackData, firePoint, bulletSpeed);

        ActiveMuzzle();
    }

    public override void ReleaseCrossBullets(AttackData attackData, Transform crossAimPoint, bool isFacingRight)
    {
        base.ReleaseCrossBullets(attackData, crossAimPoint, isFacingRight);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        float degree = 90f / (numberCrossBullet + 1);

        for (int i = 0; i < numberCrossBullet; i++)
        {
            BulletScarHGun bullet = PoolingController.Instance.poolBulletScarHGun.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletScarHGun;
            }

            bullet.Active(attackData, crossAimPoint, bulletSpeed, PoolingController.Instance.groupBullet);
            bullet.transform.Rotate(0, 0, i * degree);

            ActiveMuzzle();
        }
    }
}
