using UnityEngine;
using System.Collections;

public class GunPreviewScarH : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewScarH bullet = PoolingPreviewController.Instance.scarH.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewScarH;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
