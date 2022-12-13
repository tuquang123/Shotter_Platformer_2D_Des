using UnityEngine;
using System.Collections;

public class GunPreviewTeslaMini : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewTeslaMini bullet = PoolingPreviewController.Instance.teslaMini.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewTeslaMini;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
