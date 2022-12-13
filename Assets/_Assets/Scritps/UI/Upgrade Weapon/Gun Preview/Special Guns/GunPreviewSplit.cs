using UnityEngine;
using System.Collections;

public class GunPreviewSplit : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewSplit bullet = PoolingPreviewController.Instance.split.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewSplit;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, true, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
