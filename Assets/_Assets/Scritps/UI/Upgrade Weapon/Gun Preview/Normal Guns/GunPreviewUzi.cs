using UnityEngine;
using System.Collections;

public class GunPreviewUzi : BaseGunPreview
{
    public override void Fire()
    {
        base.Fire();

        BulletPreviewUzi bullet = PoolingPreviewController.Instance.uzi.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewUzi;
        }

        bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        Vector3 v = bullet.transform.position;
        v += firePoint.up * Random.Range(-0.15f, 0.15f);
        bullet.transform.position = v;

        ActiveMuzzle();
    }
}
