using UnityEngine;
using System.Collections;

public class GunPreviewSpread : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        for (int i = -2; i < 3; i++)
        {
            BulletPreviewSpread bullet = PoolingPreviewController.Instance.spread.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletPreviewSpread;
            }

            bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);
            bullet.transform.Rotate(0, 0, i * 10f);

            ActiveMuzzle();
        }
    }
}
