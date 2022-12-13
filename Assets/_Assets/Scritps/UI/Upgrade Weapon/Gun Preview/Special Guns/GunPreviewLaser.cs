using UnityEngine;
using System.Collections;

public class GunPreviewLaser : BaseGunPreview
{
    public Transform laserPoint;
    public BulletPreviewLaser laserPrefab;

    private BulletPreviewLaser laser;
    private bool isFiring;

    private void Awake()
    {
        CreateLaser();
    }

    private void OnEnable()
    {
        isFiring = false;
    }

    public override void Fire()
    {
        if (!isFiring)
            ActiveLaser(true);
    }

    private void ActiveLaser(bool isActive)
    {
        laser.Active(isActive);

        if (muzzle == null)
        {
            muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
        }

        if (isActive)
        {
            muzzle.Active();
        }
        else
        {
            muzzle.Deactive();
        }
    }

    private void CreateLaser()
    {
        laser = Instantiate(laserPrefab, laserPoint.position, laserPoint.rotation, firePoint);
        laser.gun = this;
    }
}
