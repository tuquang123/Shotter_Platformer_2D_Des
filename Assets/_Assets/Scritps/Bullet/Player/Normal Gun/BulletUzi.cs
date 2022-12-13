using UnityEngine;
using System.Collections;

public class BulletUzi : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletUzi.Store(this);
    }
}
