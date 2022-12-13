using UnityEngine;
using System.Collections;

public class BulletFamasGun : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletFamasGun.Store(this);
    }
}
