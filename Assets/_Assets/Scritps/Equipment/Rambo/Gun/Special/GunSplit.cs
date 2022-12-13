using UnityEngine;
using System.Collections;

public class GunSplit : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_SPLIT, level);
        baseStats = Resources.Load<SO_GunSplitStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletSplitGun bullet = PoolingController.Instance.poolBulletSplitGun.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletSplitGun;
        }

        bullet.Active(attackData, firePoint, baseStats.BulletSpeed, isSplit: true,
            splitDamage: ((SO_GunSplitStats)baseStats).DamageSplit, firstHitUnit: null, parent: PoolingController.Instance.groupBullet);

        ActiveMuzzle();
    }
}
