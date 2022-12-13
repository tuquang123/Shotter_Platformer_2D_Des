using UnityEngine;
using System.Collections;

public class GunPreviewKamePower : BaseGunPreview
{
    public float chargeTime;
    public GameObject chargeEffect;

    private float timerCharge;

    private void Update()
    {
        timerCharge += Time.deltaTime;

        if (timerCharge >= chargeTime)
        {
            timerCharge = 0;

            BulletPreviewKamePower bullet = PoolingPreviewController.Instance.kamePower.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletPreviewKamePower;
            }

            float bulletSpeed = baseStats.BulletSpeed;

            bullet.Active(firePoint, baseStats.BulletSpeed, PoolingPreviewController.Instance.group);

            ActiveMuzzle();
        }
    }
}
