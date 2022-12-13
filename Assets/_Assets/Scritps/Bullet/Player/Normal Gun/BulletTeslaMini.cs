using UnityEngine;
using System.Collections;

public class BulletTeslaMini : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletTeslaMini.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactTeslaMini, transform.position);
    }
}
