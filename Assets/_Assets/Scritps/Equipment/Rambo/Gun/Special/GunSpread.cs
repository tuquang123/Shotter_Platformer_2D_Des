using UnityEngine;
using System.Collections;

public class GunSpread : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_SPREAD, level);
        baseStats = Resources.Load<SO_GunStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        for (int i = -2; i < 3; i++)
        {
            BulletSpreadGun bullet = PoolingController.Instance.poolBulletSpreadGun.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletSpreadGun;
            }

            bullet.Active(attackData, firePoint, bulletSpeed);
            bullet.transform.Rotate(0, 0, i * 10f);
        }

        ActiveMuzzle();
    }
}
