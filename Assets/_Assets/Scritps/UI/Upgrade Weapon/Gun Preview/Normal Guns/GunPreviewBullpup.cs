using UnityEngine;
using System.Collections;

public class GunPreviewBullpup : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewBullpup bullet = PoolingPreviewController.Instance.bullpup.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewBullpup;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
