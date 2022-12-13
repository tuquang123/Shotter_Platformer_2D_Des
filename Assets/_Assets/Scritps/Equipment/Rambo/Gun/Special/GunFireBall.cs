using UnityEngine;
using System.Collections;

public class GunFireBall : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_FIRE_BALL, level);
        baseStats = Resources.Load<SO_GunFireBallStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        FireBall bullet = PoolingController.Instance.poolBulletFireBall.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as FireBall;
        }

        float timeApplyDamage = ((SO_GunFireBallStats)baseStats).TimeApplyDamage;
        float distance = ((SO_GunFireBallStats)baseStats).Distance;

        bullet.Active(attackData, firePoint, bulletSpeed, timeApplyDamage, distance);

        ActiveMuzzle();
    }
}
