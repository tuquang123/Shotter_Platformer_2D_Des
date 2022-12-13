using UnityEngine;
using System.Collections;

public class GunPreviewShotgun : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewShotgun bullet = PoolingPreviewController.Instance.shotgun.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewShotgun;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
