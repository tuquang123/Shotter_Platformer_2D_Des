using UnityEngine;
using System.Collections;

public class GunPreviewP100 : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewP100 bullet = PoolingPreviewController.Instance.p100.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewP100;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
