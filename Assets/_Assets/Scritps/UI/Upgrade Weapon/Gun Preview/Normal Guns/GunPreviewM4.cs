using UnityEngine;
using System.Collections;

public class GunPreviewM4 : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewM4 bullet = PoolingPreviewController.Instance.m4.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewM4;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        Vector3 v = bullet.transform.position;
        v += firePoint.up * Random.Range(-0.15f, 0.15f);
        bullet.transform.position = v;

        ActiveMuzzle();
    }
}
