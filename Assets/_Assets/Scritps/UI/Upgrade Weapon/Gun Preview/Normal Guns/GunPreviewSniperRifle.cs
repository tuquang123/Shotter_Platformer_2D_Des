using UnityEngine;
using System.Collections;

public class GunPreviewSniperRifle : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewSniperRifle bullet = PoolingPreviewController.Instance.sniperRifle.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewSniperRifle;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
