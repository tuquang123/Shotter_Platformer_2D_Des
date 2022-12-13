using UnityEngine;
using System.Collections;

public class GunFamas : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_FAMAS, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletFamasGun bullet = PoolingController.Instance.poolBulletFamasGun.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletFamasGun;
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
            BulletFamasGun bullet = PoolingController.Instance.poolBulletFamasGun.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletFamasGun;
            }

            bullet.Active(attackData, crossAimPoint, bulletSpeed, PoolingController.Instance.groupBullet);
            bullet.transform.Rotate(0, 0, i * degree);

            ActiveMuzzle();
        }
    }
}
