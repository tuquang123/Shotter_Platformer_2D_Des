using UnityEngine;
using System.Collections;

public class BulletMachineGunM4 : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletMachineGunM4.Store(this);
    }
}
