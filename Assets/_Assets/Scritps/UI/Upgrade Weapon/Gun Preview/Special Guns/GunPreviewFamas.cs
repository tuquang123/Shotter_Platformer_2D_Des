using UnityEngine;
using System.Collections;

public class GunPreviewFamas : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewFamas bullet = PoolingPreviewController.Instance.famas.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewFamas;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
