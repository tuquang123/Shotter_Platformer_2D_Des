using UnityEngine;
using System.Collections;

public class GunLaser : BaseGun
{
    public Transform laserPoint;
    public LaserByGun laserPrefab;
    [HideInInspector]
    public BaseUnit shooter;

    private LaserByGun laser;


    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_LASER, level);
        baseStats = Resources.Load<SO_GunLaserStats>(path);
    }

    protected override void Awake()
    {
        BaseUnit tmp = transform.root.GetComponent<BaseUnit>();

        if (tmp is Vehicle)
        {
            shooter = ((Vehicle)tmp).Player;
        }
        else
        {
            shooter = tmp;
        }

        CreateLaser();

        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, (sender, param) => ActiveLaser((bool)param));
    }

    private void OnDisable()
    {
        if (this)
            laser.Active(false);
    }

    private void OnEnable()
    {
        if (this && ((Rambo)shooter).isFiring)
        {
            ActiveLaser(true);
        }
    }

    public override void Attack(AttackData attackData) { }

    private void ActiveLaser(bool isActive)
    {
        if (this && gameObject.activeInHierarchy)
        {
            if (muzzle == null)
            {
                muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
            }

            if (isActive)
            {
                if (ammo <= 0)
                {
                    ammo = 0;
                    EventDispatcher.Instance.PostEvent(EventID.OutOfAmmo);
                    return;
                }
                else
                {
                    laser.Active(true);
                    muzzle.Active();
                }
            }
            else
            {
                laser.Active(false);
                muzzle.Deactive();
            }
        }
    }

    private void CreateLaser()
    {
        laser = Instantiate(laserPrefab, laserPoint.position, laserPoint.rotation, firePoint);
        laser.gun = this;
        laser.gameObject.SetActive(false);
    }
}
