using UnityEngine;
using System.Collections;

public class GunPreviewFireball : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewFireball bullet = PoolingPreviewController.Instance.fireball.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewFireball;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
