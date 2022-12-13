using UnityEngine;
using System.Collections;

public class BulletBullpup : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletBullpup.Store(this);
    }
}
