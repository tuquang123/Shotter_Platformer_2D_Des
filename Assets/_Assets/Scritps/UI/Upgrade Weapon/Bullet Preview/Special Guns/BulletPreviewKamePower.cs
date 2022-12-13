using UnityEngine;
using System.Collections;

public class BulletPreviewKamePower : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.kamePower.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, transform.position);
    }
}
