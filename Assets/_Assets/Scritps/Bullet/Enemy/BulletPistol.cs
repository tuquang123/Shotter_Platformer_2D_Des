using UnityEngine;
using System.Collections;

public class BulletPistol : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletPistol.Store(this);
    }
}
