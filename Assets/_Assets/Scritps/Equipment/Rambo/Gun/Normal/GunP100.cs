﻿using UnityEngine;
using System.Collections;

public class GunP100 : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_P100, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletP100 bullet = PoolingController.Instance.poolBulletP100.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletP100;
        }

        bullet.Active(attackData, firePoint, bulletSpeed);

        Vector3 v = bullet.transform.position;
        v += firePoint.up * Random.Range(-0.15f, 0.15f);
        bullet.transform.position = v;

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
            BulletP100 bullet = PoolingController.Instance.poolBulletP100.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletP100;
            }

            bullet.Active(attackData, crossAimPoint, bulletSpeed, PoolingController.Instance.groupBullet);
            bullet.transform.Rotate(0, 0, i * degree);

            ActiveMuzzle();
        }
    }
}
