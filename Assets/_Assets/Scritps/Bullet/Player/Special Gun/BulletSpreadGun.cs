using UnityEngine;
using System.Collections;

public class BulletSpreadGun : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletSpreadGun.Store(this);
    }
}
