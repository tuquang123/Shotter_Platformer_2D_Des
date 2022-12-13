using UnityEngine;
using System.Collections;

public class BulletBossProfessor : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletBossProfessor.Store(this);
    }
}
