using UnityEngine;
using System.Collections;

public class BulletRifle : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletRifle.Store(this);
    }
}
