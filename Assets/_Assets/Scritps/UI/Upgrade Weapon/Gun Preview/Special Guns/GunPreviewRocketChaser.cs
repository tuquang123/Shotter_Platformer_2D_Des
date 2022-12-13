using UnityEngine;
using System.Collections;

public class GunPreviewRocketChaser : BaseGunPreview
{
    public Transform rocketReadyPosition;

    public override void Fire()
    {
        base.Fire();

        BulletPreviewRocketChaser bullet = PoolingPreviewController.Instance.rocketChaser.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletPreviewRocketChaser;
        }

        bullet.Active(firePoint, rocketReadyPosition.position, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

        ActiveMuzzle();
    }
}
