using UnityEngine;
using System.Collections;

public class GunPreviewAWP : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewAWP bullet = PoolingPreviewController.Instance.awp.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewAWP;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
