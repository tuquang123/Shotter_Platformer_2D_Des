using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunTeslaMini : BaseGun
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_TESLA_MINI, level);
        baseStats = Resources.Load<SO_GunTeslaMiniStats>(path);
    }

    protected override void ReleaseBullet(AttackData attackData)
    {
        base.ReleaseBullet(attackData);

        if (isInfinityAmmo == false && ammo <= 0)
            return;

        BulletTeslaMini bullet = PoolingController.Instance.poolBulletTeslaMini.New();

        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefab) as BulletTeslaMini;
        }

        float stunChance = Mathf.Clamp01(((SO_GunTeslaMiniStats)baseStats).StunChance / 100f);
        bool isStun = Random.Range(0f, 1f) <= stunChance;

        if (isStun)
        {
            DebuffData debuff = new DebuffData(DebuffType.Stun, ((SO_GunTeslaMiniStats)baseStats).StunDuration);

            if (attackData.debuffs == null)
            {
                List<DebuffData> debuffs = new List<DebuffData>();
                debuffs.Add(debuff);
                attackData.debuffs = debuffs;
            }
            else
            {
                attackData.debuffs.Add(debuff);
            }
        }

        bullet.Active(attackData, firePoint, bulletSpeed);
        ActiveMuzzle();
    }
}
