using UnityEngine;
using System.Collections;

public class BulletPoisonBossVenom : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletPoisonBossVenom.Store(this);
    }
}
